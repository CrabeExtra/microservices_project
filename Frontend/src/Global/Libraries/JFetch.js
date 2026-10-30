import { API_BASE_URL, isDevelopment, SERVICE_PORT_MAP } from "./constants";


/**
 * QOL function, prefills json data type and stringifies JSON data.
 * @param {*} url 
 * @param {*} options 
 * @returns 
 */
export async function jFetch(url, options = {}) {

    if(options.body) {
        options.body = JSON.stringify(options.body);
    }

    if(url[0] === '/') {
        let baseUrl = API_BASE_URL;

        let serviceName = url.split('/')[1].toLowerCase();

        let port = SERVICE_PORT_MAP.get(serviceName);

        if(isDevelopment) {
            baseUrl = baseUrl.replace('PORT', port);
        }

        url = baseUrl + url;
    }
    
    const response = await fetch(url, {
        ...options,
        headers: {
            "Content-Type": "application/json",
            ...options.headers
        }
    });

    return response;
} 