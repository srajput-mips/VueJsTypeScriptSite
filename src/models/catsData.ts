export interface CatsData {
    Gender: string;
    Names: string[]; 
}

export interface CatsStatus { 
    loaded: boolean,
    Cats : CatsData
}