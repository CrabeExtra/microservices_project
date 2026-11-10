import { Button, Col, Row } from "react-bootstrap";
import { ArrowRight } from "react-bootstrap-icons";
import { useNavigate } from "react-router-dom";


export default function Projects() {
    
    const navigate = useNavigate();

    return (
        <Col className="align-items-center container-col">
            <Row className="justify-content-center mt-5">
                <Col xs={10}>
                    <Row className="align-items-center">
                        <Col className="experience-title-col">
                            <h1>Projects</h1>
                        </Col>
                        <Col className="experience-title-col justify-content-end mx-5 mt-5">
                            <Button onClick={() => {
                                navigate('/contact');
                            }}>
                                Get in contact!
                                <ArrowRight className="mx-1" />
                            </Button>
                        </Col>
                    </Row>
                </Col>
            </Row>
        </Col>
    )
}