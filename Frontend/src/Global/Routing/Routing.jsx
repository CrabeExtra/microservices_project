import { 
    BrowserRouter,
    Route, 
    Routes
} from "react-router-dom";

import {
    Navbar
} from "..";
import {
    Main,
    Experience,
    Skills,
    Education,
    Contact,
    Projects
} from "Pages";

export default function Routing() {

    return (
        <BrowserRouter>
            <Routes>
                <Route element={<Navbar />}>
                    <Route path="/" element={<Main />} />
                    <Route path="/experience" element={<Experience />} />
                    <Route path="/skills" element={<Skills /> } />
                    <Route path="/education" element={<Education />} />
                    <Route path="/projects" element={<Projects />} />
                    <Route path="/contact" element={<Contact />} />
                </Route>
            </Routes>
        </BrowserRouter>
    )
}