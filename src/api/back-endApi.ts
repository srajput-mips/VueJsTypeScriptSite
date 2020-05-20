
import axios from 'axios'
import { CatsData } from '@/models/catsData'

//hardcoded back-end
const baseUrl = 'http://localhost:5003';

export function getCats (): Promise<CatsData[]> {
                    
    return   axios.get(`${baseUrl}/cats`)
        .then((resp) => resp.data);
}

