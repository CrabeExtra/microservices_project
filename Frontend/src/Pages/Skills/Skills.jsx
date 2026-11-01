import { AgGridProvider, AgGridReact } from "ag-grid-react";
import { AllCommunityModule, themeQuartz, InfiniteRowModelModule  } from "ag-grid-community";
import { Col, Row } from "react-bootstrap";

import { jFetch } from "Global/Libraries";

// TODO: this is kind of messy, I should redo this as CSS.
const terminalTheme = themeQuartz.withParams({
    backgroundColor: "#000000",
    foregroundColor: "#00FF41",
    headerBackgroundColor: "#222222",
    headerTextColor: "#F2F0EA",
    borderColor: "#333333",
    accentColor: "#00FF41",
    fontFamily: "VT323",
    fontSize: 18,
    spacing: 6,
    rowHoverColor: "#111111",
});

import "./Skills.css";

import { useEffect, useState } from "react";

const modules = [AllCommunityModule, InfiniteRowModelModule];

export default function Skills() {

    const [searchedSkill, setSearchedSkill] = useState("");
    const [matchingSkills, setMatchingSkills] = useState([]);

    useEffect(() => {
        if(searchedSkill && searchedSkill !== "")
            handleSearch();
    }, []);

    const handleSearch = async () => {

    }

    const datasource = {
        getRows: async (params) => {
            const sort = params.sortModel[0];

            const query = new URLSearchParams({
                start: params.startRow,
                end: params.endRow,
                sortField: sort?.colId ?? "",
                sortDirection: sort?.sort ?? "",
                searchFilter: searchedSkill ?? ""
            });
            console.log('here')
            const response = await jFetch(`/personal/skills?${query}`);
            const data = await response.json();

            params.successCallback(data.rows, data.totalCount);
        }
    };

    // NOTE: if I need another AG grid, avoid using multiple providers.
    return (
        <AgGridProvider modules={modules}>
            <Row className="justify-content-center my-5">
                <Col xs={10}>
                    <h1>Skills</h1>
                </Col>
            </Row>
            <Row className="justify-content-center my-5">
                <Col xs={6} className="justify-content-center searchbar-col">
                    <h3>Want to know if I have a skill? Search it.</h3>
                    <input
                        className="skill-search-input"
                        value={searchedSkill ?? ""}
                        onChange={(e) => setSearchedSkill(e.target.value)}
                    />
                </Col>
            </Row>
            <Row className="grid-row justify-content-center grid-row">
                <Col className="grid-col" xs={4}>
                    <AgGridReact
                        // style
                        theme={terminalTheme}
                        // data
                        rowModelType="infinite"
                        datasource={datasource}
                        columnDefs={[
                            { field: "skill", flex: 1 },
                            { field: "type", flex: 1 },
                            { field: "Accomplishments", flex: 3 }
                        ]}
                    />
                </Col>
            </Row>
        </AgGridProvider>
    )
}