export const isDevelopment = import.meta.env.DEV

export const API_BASE_URL = isDevelopment
    ? "http://localhost:PORT/api"
    : "https://www.judebradbury.com/api";

export const SERVICE_PORT_MAP = new Map([
    ['identity', 5000],
    ['audit', 5001],
    ['activity', 5002],
    ['contact', 5003]
]);

