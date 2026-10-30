import { useState } from "react";
import { Form, Button, Col, Row } from "react-bootstrap";
import { Copy, Check } from "react-bootstrap-icons";

import "./Contact.css";

import { jFetch } from "Global/Libraries";
import { Popup, PopupType } from "Global";

export default function Contact() {
    const [email, setEmail] = useState("");
    const [message, setMessage] = useState("");
    const [copyIcon, setCopyIcon] = useState(false);

    const [cursor, setCursor] = useState({ x: 0, y: 0 });
    

    const [showSuccess, setShowSuccess] = useState(false);
    const [showError, setShowError] = useState(false);

    const emailInvalid =
        email.length > 0 &&
        !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);

    const handleSubmit = async (event) => {
        event.preventDefault();

        if (emailInvalid || !email.trim() || !message.trim()) {
            return;
        }

        // TODO: Send email and message to your API.
        const response = await jFetch("/contact/interaction", {
            method: "POST",
            body: {
                email: email,
                body: message.replaceAll("\n", "<p></p>") // preserves newline in the email.
            }
        });

        if(response.ok) {
            setShowSuccess(true);
        } else {
            setShowError(true);
        }
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
            <Col xs={6}>
                <Popup 
                    header={"Error"} 
                    body={"There was an error transmitting your message! Please try again, or contact me directly via the provided address."}  
                    show={showError}
                    onClose={() => setShowError(false)}
                    type={PopupType.error}
                    options={{ delay: 6000 }} // increase delay for reading the extra text.
                />
                <Popup 
                    header={"Success!"} 
                    body={"You may check your email for confirmation."}  
                    show={showSuccess}
                    onClose={() => setShowSuccess(false)}
                    type={PopupType.success}
                />
                <Row className="justify-content-center">
                    <h1>Contact me</h1>
                    <p>Feel free to contact me if you have roles or projects for me to work on or if you would just like to connect. 
                        <br/> Leave me a message below or contact me via my email address (click to copy address):
                    </p> 
                    <p>
                        <span
                            className="copy-hover"
                            onMouseMove={(e) => setCursor({ x: e.clientX, y: e.clientY })}
                            onClick={() => copied()}
                        >
                            judebb4@gmail.com
                        </span>

                        <span
                            className="copy-popup"
                            style={{
                                left: cursor.x + 12,
                                top: cursor.y + 12
                            }}
                        >
                            {copyIcon ? <Check size={14} /> : <Copy size={14} />}
                        </span>
                    </p>
                    <p>
                        Thank you!  
                    </p>
                </Row>
                <Row className="justify-content-center">
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
                </Row>
            </Col>
        </Row>
    );

}