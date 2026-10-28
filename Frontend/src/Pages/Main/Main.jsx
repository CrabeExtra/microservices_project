import { 
    Button,
    Col,
    Ratio,
    Row 
} from "react-bootstrap";
import { 
    useEffect, 
    useRef, 
    useState 
} from "react";
import { useNavigate } from "react-router-dom";
import { AmbientGlow } from "video-ambient-glow";
import { ArrowRight } from "react-bootstrap-icons";

import "./Main.css";

import calcSource from "@/assets/splash_clips/calculator.mp4";
import codeSource from "@/assets/splash_clips/code_structure.mp4";
import gridLogger from "@/assets/splash_clips/grid_logger.mp4";
import kanban from "@/assets/splash_clips/kanban.mp4";


export default function Main() {

    const navigate = useNavigate();

    const FADE_OUT_VIDEO_SECONDS = 2;

    const [fading, setFading] = useState(false);
    const [activeIndex, setActiveIndex] = useState(0);
    
    const videoRef = useRef(null);

    const videoSources = [
        codeSource,
        calcSource,
        gridLogger,
        kanban
    ]

    useEffect(() => {
        if (!videoRef.current) return;

        const glow = new AmbientGlow(videoRef.current, {
            blur: 10,
            opacity: 0.3,
        });

        return () => glow.destroy();
    }, [fading]);
    
    return (
        <Row 
            className="mt-5 justify-content-center align-items-center main-row"
        >
            <Col xs={4} className="main-cols px-2">
                <Row>
                    <h1>
                        JUDE BRADBURY
                    </h1>
                </Row>
                
                <Row>
                    Senior Software Engineer with 4+ years of experience from startup to enterprise environments, 
                    with a consistent track record of technical leadership, mentoring, and reliable end-to-end project delivery. 
                    Experienced in full-stack development, systems architecture, distributed services, database design, and production operations.
                </Row>

                <Row className="justify-content-center align-items-center" style={{ height: "200px" }}>
                    <Button onClick={() => {
                        navigate('/experience');
                    }}>
                        Experience
                        <ArrowRight className="mx-1" />
                    </Button>
                </Row>
            </Col>
            <Col xs={4} className="main-cols px-2">
                <Ratio
                    className="my-3 splash-video"
                    style={{
                        opacity: fading ? 0 : 1,
                        transition: `opacity ${FADE_OUT_VIDEO_SECONDS}s linear`, 
                    }}
                    aspectRatio={"16x9"}
                >
                    <video 
                        key={activeIndex}
                        ref={videoRef}
                        muted 
                        autoPlay
                        onTimeUpdate={(e) => {
                            const video = e.currentTarget;
                            if (video.duration - video.currentTime <= FADE_OUT_VIDEO_SECONDS) {
                                setFading(true);
                            }
                        }}
                        onEnded={(e) => {
                            e.currentTarget.currentTime = 0;
                            setFading(false);
                            setActiveIndex(prev => {
                                const newValue = (prev + 1) % videoSources.length;
                                if(newValue > videoSources.length) return 0; // this isn't really necessary, I just didnt want someone who was waiting with my webpage open in their browser for 100 years to run into number > maxInt
                                console.log(newValue)
                                return newValue;
                            });
                            e.currentTarget.play();
                        }}
                        style={{
                            width: "100%", 
                            height: "100%", 
                            objectFit: "contain",
                        }}
                    >
                        <source src={videoSources[activeIndex]} type="video/mp4" />
                    </video>
                </Ratio>
            </Col>
        </Row>
    )
}