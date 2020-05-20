import * as c from '@/api/fun-data'

describe('currency model', () => {
    describe('convert function', () => {
        it('should be able to authenticate', () => {
            const output = c.authenticate('test','tt','')
            expect(output.then(function(result) { })).toEqual(140)
        })
 
    })
})
