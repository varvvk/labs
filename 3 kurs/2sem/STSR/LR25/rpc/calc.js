module.exports = {
    sum: (params) => {
        if(!Array.isArray(params)) {
            throw {
                code: -32602,
                message: 'параметры должны быть массивом'
            }
        }
        return params.reduce((a, b) => a + b, 0)
    },

    mul: (params) => {
        if(!Array.isArray(params)) {
            throw {
                code : -32602,
                message: 'параметры должны быть массивом'
            }
        }
        return params.reduce((a,b) => a * b, 1)
    },

    div: (params) => {
        if(!Array.isArray(params) || params.length !==2) {
            throw {
                code: -32602,
                message: 'параметры должны быть массивом из 2 чисел'
            }
        }
        const [x, y] = params
            if(y === 0) {
                throw {
                code: -32602,
                message: 'деление на ноль'
            }
        }
        return x/y
    },

    proc: (params) => {
        if(!Array.isArray(params) || params.length !== 2) {
            throw {
                code: -32602,
                message: 'параметры должны быть массивом из двух чисел'
            }
        }
        const [x,y] = params
        if(y===0) {
            throw {
                code: -32602,
                message:'деление на ноль'
            }
        }
        return (x/y)*100
    }
}