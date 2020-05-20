import axios from 'axios'

import { AuthenticationResult } from '@/models/user'
import moment from 'moment'    

//boiler plate
export function handleApiError (error: any): string {
    if (error.response) {
        if (error.response.data.Message) return error.response.data.Message
        return error.response.data || 'An error occurred'
    }
    if (error.request) { return 'No response to the request was received from the API' }
    if (error.message) { return error.message }
    return error
}


export function login (uid: string, pwd: string, baseUrl: string): AuthenticationResult { 
    
    //hardcoded
    const userLogged= {  FirstName: 'test',   
          Email: 'admin@s.com', Name: 'test',
              LastName: 'singh',  
              Id: 's@s.com'  };
  
      if(uid == 'test'){
          
          const userRes = { success: true, user: userLogged,message :'success', timestamp: moment()};
          return userRes;
          }
          
          const userRes2 = { success: false, user: userLogged,message :'failed', timestamp: moment()};
          return  userRes2
  }


export function authenticate (uid: string, pwd: string, baseUrl: string): Promise<AuthenticationResult> { 
    
  //hardcoded
  const userLogged= {  FirstName: 'test',   
        Email: 'admin@s.com', Name: 'test',
            LastName: 'singh',  
            Id: 's@s.com'  };

    if(uid == 'test'){
        
        const userRes = { success: true, user: userLogged,message :'success', timestamp: moment()};
        return Promise.resolve(userRes)
        }
        
        const userRes2 = { success: false, user: userLogged,message :'failed', timestamp: moment()};
        return Promise.resolve(userRes2)
}




export function auth (uid: string, pwd: string, baseUrl: string): Promise<AuthenticationResult> { 
  //if back-end is to be called
    return axios.post(baseUrl + '/auth', {uid, pwd})
    .then(resp => resp.data)
}
 
  