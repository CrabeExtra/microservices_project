import { Button, Col, Row } from "react-bootstrap";
import { ArrowRight } from "react-bootstrap-icons";
import { useNavigate } from "react-router-dom";

import "./Education.css";

export default function Education() {
    const navigate = useNavigate();

    return (
        <Col className="align-items-center container-col">
            <Row className="justify-content-center mt-5">
                <Col xs={10}>
                    <Row className="align-items-center">
                        <Col className="experience-title-col">
                            <h1>Education</h1>
                        </Col>
                        <Col className="experience-title-col justify-content-end mx-5 mt-5">
                            <Button onClick={() => {
                                navigate('/projects');
                            }}>
                                Projects
                                <ArrowRight className="mx-1" />
                            </Button>
                        </Col>
                    </Row>
                </Col>
            </Row>

            <Row className="justify-content-center mt-5">
                
                
                <Col xs={8}>
                    <Row style={{display: "inline"}}>
                        <h2 style={{display: "inline", marginRight: "0.5rem"}}>
                            University of Tasmania 
                        </h2>
                        <h3 style={{display: "inline"}}>
                            - Bachelor of Computer Science
                        </h3>
                    </Row>
                    <Row>
                        In 2021 I graduated with a Bachelor's in Computer Science with competitive grades and a wide range of academic experience, from project management to embedded systems. 
                        Some of my extracaricular activies while studying include becoming the president of the engineering society, and working on UTAS' first competition electric Formula vehicle. 
                    </Row>
                    <Row>
                        Some of my skills acquired and practiced while studying listed below.
                    </Row>
                    <Row className="mt-2">
                        <ul>
                            {
                                [
                                    "Artificial intelligence and machine learning (Python)",
                                    "ICT project management",
                                    "Mobile app development (Java, Swift)",
                                    "Big data",
                                    "Cloud computing",
                                    "Concurrent programming (C++)",
                                    "Computer graphics and animation programming (Java)",
                                    "Embedded systems (C)",
                                    "Data structures and algorithms"
                                ].map(item => (<li>{item}</li>))
                            }
                        </ul>
                    </Row>
                    <Row>
                        The above briefly summarises my science degree. I initially began studying as a double degree, so this included completing most of the classes required for a Bachelor of Engineering (hons) specialising in computer systems with competitive grades. 
                        I wanted to enter the workforce quickly though, and so dropped my honours project to start work in the industry. I am, however, confident in my abilities here as well. I became very competent in a number of skills unassociated with the direction that my career
                        has taken as of 2026. Some of these skills include the following.
                    </Row>
                    <Row className="mt-2">
                        <ul>
                            {
                                [
                                    "Mathematics (Calculus, numerical methods Fourier/Laplace transforms)",
                                    "Mathematical signal processing (linear and stochastic systems)",
                                    "Circuit theory and analysis",
                                    "Control and process control systems",
                                    "Computer and data networks",
                                    "Digital and Analogue electronics",
                                    "Reliability calculations and risk analyses",
                                    "Operation and analysis of high power electrical machines",
                                    "Kinematics (statics and dynamics)",
                                ].map(item => (<li>{item}</li>))
                            }
                        </ul>
                    </Row>
                    <Row>
                        After graduating, and while working I spent my spare time studying out of interest. In November of 2025, I was granted a Bachelor's majoring in psychology. I hope this may be applicable to managerial and leadership roles in the future.
                    </Row>
                </Col>
                
            </Row>
        </Col>
    )
}