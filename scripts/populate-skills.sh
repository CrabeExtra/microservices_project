#!/usr/bin/env bash
set -Eeuo pipefail

API_ROOT="${PERSONAL_SKILLS_API_ROOT:-http://localhost:5004/api/personal/skills}"
ADMIN_KEY="${PERSONAL_API_ADMIN_KEY:?Set PERSONAL_API_ADMIN_KEY before running this script}"
JSON_FILE="${1:-"$(dirname -- "${BASH_SOURCE[0]}")/new_skills.json"}"

command -v jq >/dev/null 2>&1 || {
    echo "Error: jq is required. Install it with: sudo pacman -S jq" >&2
    exit 1
}
command -v curl >/dev/null 2>&1 || {
    echo "Error: curl is required. Install it with: sudo pacman -S curl" >&2
    exit 1
}
[[ -f "$JSON_FILE" ]] || {
    echo "Error: JSON file not found: $JSON_FILE" >&2
    exit 1
}

API_ROOT="${API_ROOT%/}"

post() {
    local endpoint="$1"
    local data="$2"
    local url="$API_ROOT"
    [[ -z "$endpoint" ]] || url="$API_ROOT/$endpoint"

    jq -n \
        --arg key "$ADMIN_KEY" \
        --argjson data "$data" \
        '{Key: $key, Data: $data}' |
        curl --fail-with-body --silent --show-error \
            --request POST \
            --header "Content-Type: application/json; charset=utf-8" \
            --data-binary @- \
            "$url" >/dev/null
}

jq -e '(.skills | type == "array")' "$JSON_FILE" >/dev/null || {
    echo "Error: expected a top-level .skills array in $JSON_FILE" >&2
    exit 1
}

# ---- Derived counts (for logging) ----
skill_count="$(jq '.skills | length' "$JSON_FILE")"
synonym_count="$(jq '[.skills[].synonyms // [] | length] | add // 0' "$JSON_FILE")"

# Group nested accomplishments by text; aggregate skill names.
# Result: [{ company, text, skillNames: [...] }, ...]
unique_accomplishments="$(
    jq -c '
        [ .skills[] as $s
          | ($s.accomplishments // [])[]
          | { company: (.company // ""), text: (.text // ""), skill: $s.name }
        ]
        | group_by(.text)
        | map({
            company:    .[0].company,
            text:       .[0].text,
            skillNames: ([.[].skill] | unique)
          })
        | sort_by(.text)
    ' "$JSON_FILE"
)"
accomplishment_count="$(jq 'length' <<< "$unique_accomplishments")"

# ---- Skills ----
echo "Creating $skill_count skills..."

mapfile -t skills < <(jq -c '.skills[]' "$JSON_FILE")

for skill in "${skills[@]}"; do
    name="$(jq -r '.name // ""' <<< "$skill")"
    echo "  Skill: $name"

    skill_data="$(jq -c '{
        Name: .name,
        Type: .type,
        Description: .description
    }' <<< "$skill")"

    post "" "$skill_data"
done

# ---- Accomplishments (deduped by text) ----
echo "Creating $accomplishment_count unique accomplishments..."

while IFS= read -r accomplishment; do
    company="$(jq -r '.company // ""' <<< "$accomplishment")"
    text="$(jq -r '.text // ""' <<< "$accomplishment")"
    full_text="$company: $text"
    skill_names="$(jq -c '.skillNames' <<< "$accomplishment")"
    n_skills="$(jq '.skillNames | length' <<< "$accomplishment")"

    echo "  Accomplishment: $company -> $n_skills skills"

    accomplishment_data="$(jq -cn \
        --arg text "$full_text" \
        --argjson skillNames "$skill_names" \
        '{Text: $text, SkillNames: $skillNames}')"

    post "accomplishment" "$accomplishment_data"
done < <(jq -c '.[]' <<< "$unique_accomplishments")

# ---- Synonyms ----
echo "Creating $synonym_count synonyms..."

for skill in "${skills[@]}"; do
    name="$(jq -r '.name // ""' <<< "$skill")"

    while IFS= read -r synonym; do
        echo "  Synonym: $synonym -> $name"

        synonym_data="$(jq -cn \
            --arg synonym "$synonym" \
            --arg skill "$name" \
            '{Name: $synonym, SkillName: $skill}')"

        post "synonym" "$synonym_data"
    done < <(jq -r '.synonyms // [] | .[]' <<< "$skill")
done

echo "Skill seeding completed successfully."