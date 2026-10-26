import { 
    BrowserRouter,
    Route, 
    Routes
} from "react-router-dom";

import {
    Navbar
} from "../../Global";
import {
    Main,
    Experience,
    Skills,
    Education,
    Contact
} from "../";

export default function Routing() {

    return (
        <BrowserRouter>
            <Routes>
                <Route element={<Navbar />}>
                    <Route path="/" element={<Main />} />
                    <Route path="/experience" element={<Experience />} />
                    <Route path="/skills" element={<Skills /> } />
                    <Route path="/education" element={<Education />} />
                    <Route path="/contact" element={<Contact />} />
                </Route>
            </Routes>
        </BrowserRouter>
    )
}