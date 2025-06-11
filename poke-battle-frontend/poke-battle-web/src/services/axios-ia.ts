import axios from "axios";

const aiClient = axios.create({
    baseURL: 'http://localhost:5070/api/gen-ai',
    timeout: 10000
});
export default aiClient;