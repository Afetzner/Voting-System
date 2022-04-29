import axios from "axios";

const url = "https://localhost:7237/api/polls";

class pollService {
    getPolls() {
        return axios.get(url);
    }
}

export default new pollService();