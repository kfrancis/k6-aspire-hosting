import http from "k6/http";
import { sleep } from "k6";

export default function () {
    // Get API information from environment
    const apiHost = __ENV.APP_HOST;
    const apiScheme = __ENV.APP_ENDPOINT_SCHEME;

    // Construct the base URL - in Aspire, services can be accessed by their resource name
    const baseUrl = `${apiScheme}://${apiHost}`;

    console.log(`Testing API at: ${baseUrl}`);

    const response = http.get(`${baseUrl}/weatherforecast`);

    check(response, {
        'status is 200': (r) => r.status === 200
    });

    sleep(1);
}
