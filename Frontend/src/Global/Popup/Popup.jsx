import {
    ToastContainer,
    Toast
} from "react-bootstrap";

import "./Popup.css";

// maybe I should have used typescript..
export class PopupType {
    static default = "default";
    static success = "success";
    static error = "error";
}

export default function Popup({header, body, show, onClose, type = PopupType.default, options = {}}) {

    return (
        <ToastContainer className={`p-3`} position="bottom-end">
            <Toast
                show={show}
                onClose={onClose}
                delay={options.delay ? options.delay : 3000}
                autohide
                className={`${type}`}
            >
                <Toast.Header className={`${type}`} >
                    <strong className="me-auto">{header}</strong>
                </Toast.Header>
                <Toast.Body className={`${type}`} >
                    {body}
                </Toast.Body>
            </Toast>
        </ToastContainer>
    )
}