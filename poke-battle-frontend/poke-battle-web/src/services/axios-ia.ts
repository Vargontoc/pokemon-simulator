import axios from "axios";

const aiClient = axios.create({
    baseURL: 'http://localhost:6000/api/gen-ai',
    timeout: 5000
});
export default aiClient;