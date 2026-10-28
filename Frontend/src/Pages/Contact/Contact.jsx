import { useEffect, useState } from "react";
import { Form, Button, Col, Row } from "react-bootstrap";

import "./Contact.css";
import { Copy } from "react-bootstrap-icons";

export default function Contact() {
    const [email, setEmail] = useState("");
    const [message, setMessage] = useState("");
    const [copyIcon, setCopyIcon] = useState(false);

    const emailInvalid =
        email.length > 0 &&
        !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);

    const handleSubmit = async (event) => {
        event.preventDefault();

        if (emailInvalid || !email.trim() || !message.trim()) {
            return;
        }

        // TODO: Send email and message to your API.
        const response = await fetch("http://localhost:5003/api/interaction", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                email: email,
                body: message
            })
        });
    };

    const copied = () => {
        
        navigator.clipboard.writeText("judebb4@gmail.com")
        setCopyIcon(true);
        setTimeout(() => {
            setCopyIcon(false);
        }, 1000)
    }

    return (
        <Row className="justify-content-center align-items-center contact-row">
            <Row className="justify-content-center">
                <Col xs={6}>
                    <h1>Contact me</h1>
                    <p>Feel free to contact me if you have roles or projects for me to work on or if you would just like to connect. 
                        <br/><br/> Leave me a message below or contact me via my email address:
                    </p> 
                    <p>
                        <span
                            title={copyIcon ? <Copy /> : "Click to copy"}
                            style={{
                                cursor: "pointer",
                                width: "fit-content"
                            }} 
                            onClick={() => {
                                copied();
                            }}>judebb4@gmail.com</span> 
                    </p>
                    <p>
                        Thank you!  
                    </p>
                </Col>
            </Row>
            <Row className="justify-content-center">
                <Col xs={6}>
                    <Form onSubmit={handleSubmit}>
                        <Form.Group className="mb-3" controlId="contactEmail">
                            <Form.Label>Email address</Form.Label>
                            <Form.Control
                                type="email"
                                placeholder="you@example.com"
                                value={email}
                                onChange={(event) => setEmail(event.target.value)}
                                isInvalid={emailInvalid}
                                required
                            />
                            <Form.Control.Feedback type="invalid">
                                Please enter a valid email address.
                            </Form.Control.Feedback>
                        </Form.Group>

                        <Form.Group className="mb-3" controlId="contactMessage">
                            <Form.Label>Message</Form.Label>
                            <Form.Control
                                as="textarea"
                                rows={5}
                                placeholder="Your message..."
                                value={message}
                                onChange={(event) => setMessage(event.target.value)}
                                required
                            />
                        </Form.Group>

                        <Button type="submit" disabled={emailInvalid || !email.trim() || !message.trim()}>
                            Send message
                        </Button>
                    </Form>
                </Col>
            </Row>
        </Row>
    );

}