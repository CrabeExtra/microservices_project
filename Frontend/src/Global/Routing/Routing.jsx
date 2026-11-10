import { 
    BrowserRouter,
    Outlet,
    Route, 
    Routes
} from "react-router-dom";
import { AgGridProvider } from "ag-grid-react";
import { AllCommunityModule, InfiniteRowModelModule  } from "ag-grid-community";

import {
    Navbar
} from "..";
import {
    Main,
    Experience,
    Skills,
    Education,
    Contact,
    Projects,
    ProjectInfo
} from "Pages";

const modules = [AllCommunityModule, InfiniteRowModelModule];

const GridProvider = () => (
    <AgGridProvider modules={modules}>
        <Outlet />
    </AgGridProvider>
);

export default function Routing() {

    return (
        <BrowserRouter>
            <Routes>
                <Route element={<Navbar />}>
                    <Route path="/" element={<Main />} />
                    <Route path="/experience" element={<Experience />} />

                    <Route element={<GridProvider />} >
                        <Route path="/skills" element={<Skills /> } />
                        <Route path="/projects/:projectName" element={<ProjectInfo />} />
                    </Route>
                    
                    <Route path="/education" element={<Education />} />
                    <Route path="/projects" element={<Projects />} />
                    <Route path="/contact" element={<Contact />} />
                </Route>
            </Routes>
        </BrowserRouter>
    )
}