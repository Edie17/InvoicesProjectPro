/*  _____ _______         _                      _
 * |_   _|__   __|       | |                    | |
 *   | |    | |_ __   ___| |___      _____  _ __| | __  ___ ____
 *   | |    | | '_ \ / _ \ __\ \ /\ / / _ \| '__| |/ / / __|_  /
 *  _| |_   | | | | |  __/ |_ \ V  V / (_) | |  |   < | (__ / /
 * |_____|  |_|_| |_|\___|\__| \_/\_/ \___/|_|  |_|\_(_)___/___|
 *                                _
 *              ___ ___ ___ _____|_|_ _ _____
 *             | . |  _| -_|     | | | |     |  LICENCE
 *             |  _|_| |___|_|_|_|_|___|_|_|_|
 *             |_|
 *
 *   PROGRAMOVÁNÍ  <>  DESIGN  <>  PRÁCE/PODNIKÁNÍ  <>  HW A SW
 *
 * Tento zdrojový kód je součástí výukových seriálů na
 * IT sociální síti WWW.ITNETWORK.CZ
 *
 * Kód spadá pod licenci prémiového obsahu a vznikl díky podpoře
 * našich členů. Je určen pouze pro osobní užití a nesmí být šířen.
 * Více informací na http://www.itnetwork.cz/licence
 */

/**
 * Utility function for making API requests with proper error handling.
 *
 * @param {string} url - API endpoint URL
 * @param {Object} requestOptions - Fetch request options
 * @returns {Promise} - Promise that resolves with the response data
 */
const fetchData = (url, requestOptions) => {
    return fetch(url, requestOptions)
        .then((response) => {
            if (!response.ok) {
                throw new Error(`Network response was not ok: ${response.status} ${response.statusText}`);
            }

            if (requestOptions.method !== 'DELETE')
                return response.json();
        });
};

/**
 * Performs a GET request to the API.
 * 
 * @param {string} url - API endpoint URL
 * @param {Object} params - URL query parameters
 * @returns {Promise} - Promise that resolves with the response data
 */
export const apiGet = (url, params) => {
    const filteredParams = Object.fromEntries(
        Object.entries(params || {}).filter(([_, value]) => value != null)
    );

    const apiUrl = `${url}?${new URLSearchParams(filteredParams)}`;
    const requestOptions = {
        method: "GET",
    };

    return fetchData(apiUrl, requestOptions);
};

/**
 * Performs a POST request to the API.
 * 
 * @param {string} url - API endpoint URL
 * @param {Object} data - Data to send in the request body
 * @returns {Promise} - Promise that resolves with the response data
 */
export const apiPost = (url, data) => {
    const requestOptions = {
        method: "POST",
        headers: {"Content-Type": "application/json"},
        body: JSON.stringify(data),
    };

    return fetchData(url, requestOptions);
};

/**
 * Performs a PUT request to the API.
 * 
 * @param {string} url - API endpoint URL
 * @param {Object} data - Data to send in the request body
 * @returns {Promise} - Promise that resolves with the response data
 */
export const apiPut = (url, data) => {
    const requestOptions = {
        method: "PUT",
        headers: {"Content-Type": "application/json"},
        body: JSON.stringify(data),
    };

    return fetchData(url, requestOptions);
};

/**
 * Performs a DELETE request to the API.
 *
 * @param {string} url - API endpoint URL
 * @returns {Promise} - Promise that resolves when the delete is complete
 */
export const apiDelete = (url) => {
    const requestOptions = {
        method: "DELETE",
    };

    return fetchData(url, requestOptions);
};

/**
 * Filters invoices using GET query parameters.
 *
 * @param {Object} params - Filter parameters (sellerId, buyerId, product, minPrice, maxPrice, limit, invoiceNumber, issuedFrom, issuedTo)
 * @returns {Promise} - Promise that resolves with filtered invoice list
 */
export const apiFilterInvoices = (params) => {
    return apiGet("/api/invoices", params);
};