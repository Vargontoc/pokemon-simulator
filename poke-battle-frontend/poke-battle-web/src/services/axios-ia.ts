import axios from "axios";

const aiClient = axios.create({
    baseURL: 'http://localhost:5196/api/gen-ai',
    timeout: 10000
});
export default aiClient;