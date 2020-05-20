// system wide Initialization
export const INITIALIZE_AFTER_AUTH = 'INITIALIZE_AFTER_AUTH' // happens after any authentication success
export const INITIALIZE = 'INITIALIZE' // happens after config loads, after authentication attempt
export const SHUTDOWN = 'SHUTDOWN' // window.onunload event, use to write to localData etc

// auth Actions
export const LOG_IN = 'LOG_IN'
export const LOG_IN_HARDCODED = 'LOG_IN_HARDCODED' 
export const LOG_OUT = 'LOG_OUT'


export const GET_CATS = 'GET_CATS' 