import * as c from '@/api/back-endApi'

describe('Back-end testing', () => {
    describe('cats api result test', () => {
             
        test(`Mocking with spyon test`, () => { 
            const spy = jest.spyOn(c, 'getCats');
            const result = c.getCats();
            expect(c.getCats).toHaveBeenCalled() 
        })
        ,
        it('2 rows should be returned in the result from api one for each gender', () => {

            return c.getCats().then(result => {
                console.log(result);
                console.log(result.length); 
                expect(result.length).toBe(2);
              })

              
        })
 
    })
   
})
