import http from "k6/http";
import { check, sleep } from "k6";

export const options = {
    stages: [
        { duration: "20s", target: 5 },
        { duration: "40s", target: 20 },
        { duration: "30s", target: 10 },
        { duration: "20s", target: 0 }
    ],
    thresholds: {
        http_req_failed: ["rate<0.25"],
        http_req_duration: ["p(95)<2500"],
        checks: ["rate>0.7"]
    }
};

function randomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

export default function () {

    // Get API information from environment
    const apiHost = __ENV.APP_HOST;
    const apiScheme = __ENV.APP_ENDPOINT_SCHEME;

    // Construct the base URL
    const baseUrl = `${apiScheme}://${apiHost}`;

    const days = randomInt(1, 14);
    const slowDelayMs = randomInt(150, 2000);
    const failRate = randomInt(10, 45);

    const baselineResponse = http.get(`${baseUrl}/weatherforecast`, {
        tags: { name: "weatherforecast_default" }
    });
    check(baselineResponse, {
        "default status is 200": (r) => r.status === 200,
        "default has data": (r) => Array.isArray(r.json()) && r.json().length > 0
    });

    const variableDaysResponse = http.get(`${baseUrl}/weatherforecast/${days}`, {
        tags: { name: "weatherforecast_variable_days" }
    });
    check(variableDaysResponse, {
        "variable-days status is 200": (r) => r.status === 200,
        "variable-days size matches": (r) => Array.isArray(r.json()) && r.json().length === days
    });

    const slowResponse = http.get(`${baseUrl}/weatherforecast/slow?delayMs=${slowDelayMs}`, {
        tags: { name: "weatherforecast_slow" }
    });
    check(slowResponse, {
        "slow status is 200": (r) => r.status === 200,
        "slow endpoint was delayed": (r) => r.timings.duration >= slowDelayMs * 0.7
    });

    const flakyResponse = http.get(`${baseUrl}/weatherforecast/flaky?failRate=${failRate}`, {
        tags: { name: "weatherforecast_flaky" }
    });
    check(flakyResponse, {
        "flaky status is 200 or 503": (r) => r.status === 200 || r.status === 503
    });

    sleep(Math.random() * 1.5 + 0.2);
}
