import { useState } from "react";
import { Row, Col, Card, Modal } from "react-bootstrap";

import experienceData from "./experience.json";
import "./Experience.css";

export default function Experience() {
    const [selectedRole, setSelectedRole] = useState(null);

    return (
        <>
            <Row className="justify-content-center">
                <Col xs={10}>
                    <h1>My Experience</h1>
                </Col>
            </Row>

            <Row className="justify-content-center mt-4">
                <Col xs={10}>
                    <div className="experience-grid">
                        {experienceData.roles.map((role, index) => (
                            <Card
                                className="experience-card"
                                key={index}
                                onClick={() => setSelectedRole(role)}
                                role="button"
                                tabIndex={0}
                                onKeyDown={(e) => {
                                    if (e.key === "Enter" || e.key === " ") {
                                        e.preventDefault();
                                        setSelectedRole(role);
                                    }
                                }}
                            >
                                <Card.Header>
                                    <Card.Title className="experience-company">
                                        {role.company}
                                    </Card.Title>
                                </Card.Header>

                                <Card.Body>
                                    <Row className="align-items-center">
                                        <Col>
                                            <Card.Subtitle className="experience-title">
                                                {role.title}
                                            </Card.Subtitle>
                                        </Col>

                                        <Col xs="auto">
                                            <span className="experience-interval">
                                                {role.interval}
                                            </span>
                                        </Col>
                                    </Row>

                                    <ul className="experience-points">
                                        {role.points.map((point, i) => (
                                            <li key={i}>{point}</li>
                                        ))}
                                    </ul>
                                </Card.Body>
                            </Card>
                        ))}
                    </div>
                </Col>
            </Row>

            <Modal
                show={selectedRole !== null}
                onHide={() => setSelectedRole(null)}
                centered
                scrollable
                size="lg"
                contentClassName="experience-modal"
            >
                {selectedRole && (
                    <>
                        <Modal.Header closeButton>
                            <Modal.Title>
                                <div className="experience-company">
                                    {selectedRole.company}
                                </div>
                                <div className="experience-modal-title">
                                    {selectedRole.title}
                                </div>
                                <small className="experience-interval">
                                    {selectedRole.interval}
                                </small>
                            </Modal.Title>
                        </Modal.Header>

                        <Modal.Body>
                            <ul className="experience-points">
                                {selectedRole.points.map((point, i) => (
                                    <li key={i}>{point}</li>
                                ))}
                            </ul>
                        </Modal.Body>
                    </>
                )}
            </Modal>
        </>
    );
}