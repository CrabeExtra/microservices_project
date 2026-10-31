import { 
    Nav,
    Navbar as NavbarReact
} from "react-bootstrap";
import { 
    Link, 
    Outlet
} from "react-router-dom";

import "./Navbar.css";


export default function Navbar() {

    return (
        <>
            <div className="navbar-container">
                <div className="mx-4">
                    <NavbarReact data-bs-theme="dark" expand="lg" className="bg-body-primary">
                        <NavbarReact.Brand as={Link} to="/">JUDE BRADBURY</NavbarReact.Brand>
                        <NavbarReact.Toggle aria-controls="basic-navbar-nav" />
                        <NavbarReact.Collapse id="basic-navbar-nav">
                            <Nav className="me-auto">
                                <Nav.Link as={Link} to="/experience">Experience</Nav.Link>
                                {/*<Nav.Link as={Link} to="/skills">Skills</Nav.Link>
                                <Nav.Link as={Link} to="/education">Education</Nav.Link>
                                <Nav.Link as={Link} to="/projects">Projects</Nav.Link> */}
                                <Nav.Link as={Link} to="/contact">Contact</Nav.Link>
                            </Nav>
                        </NavbarReact.Collapse>
                    </NavbarReact>
                </div>
            </div>
            <div className="navbar-spacer"></div>
            <Outlet />
        </>
        
    );
}