import { AgGridProvider, AgGridReact } from "ag-grid-react";
import { AllCommunityModule, themeQuartz, InfiniteRowModelModule  } from "ag-grid-community";
import { Button, Col, Modal, Row } from "react-bootstrap";

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
    const [selectedRow, setSelectedRow] = useState(null);

    const onRowClicked = (e) => {
        if (e.data) setSelectedRow(e.data);
    };

    const datasource = {
        getRows: async (params) => {

            const colToField = (col) => {
                switch (col) {
                    case "skill":       return "Name";
                    case "information": return "Description";
                    case "type":        return "Type";
                    default:            return col ?? "";
                }
            };

            const sort = params.sortModel[0];
            const pageSize = params.endRow - params.startRow;
            const page = Math.floor(params.startRow / pageSize) + 1;
            
            const query = new URLSearchParams({
                page: String(page),
                pageSize: String(pageSize),
                sortField: colToField(sort?.colId),
                asc: String(sort?.sort === "asc" || sort?.sort === undefined),   // backend expects bool
                search: searchedSkill ?? "",
            });

            const response = await jFetch(`/personal/skills?${query}`);
            const data = await response.json();

            params.successCallback(data.items.map(item => ({
                skill: item.name,
                type: item.type,
                information: item.description,
                accomplishments: item.accomplishments
            })), data.totalCount);
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
                    Click a skill to learn more about it.
                    <AgGridReact
                        // style
                        theme={terminalTheme}
                        // data
                        rowModelType="infinite"
                        datasource={datasource}
                        onRowClicked={onRowClicked}
                        columnDefs={[
                            { field: "skill", flex: 1 },
                            { field: "type", flex: 1 },
                            { field: "information", flex: 3 }
                        ]}
                    />
                </Col>
            </Row>
            <Modal
                show={selectedRow !== null}
                onHide={() => setSelectedRow(null)}
                centered
                size="lg"
            >
                <Modal.Header closeButton>
                    <Modal.Title>{selectedRow?.skill}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <p>{selectedRow?.information}</p>
                    {selectedRow?.accomplishments.length > 0 ? 
                        <ul>
                            {(selectedRow?.accomplishments ?? []).map((a, i) => (
                                <li key={i}>{a}</li>
                            ))}
                        </ul> 
                    : 
                        <p>
                            I haven't added a list of notable accomplishments here yet!
                        </p>
                    }
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setSelectedRow(null)}>
                        Close
                    </Button>
                </Modal.Footer>
            </Modal>
        </AgGridProvider>
    )
}