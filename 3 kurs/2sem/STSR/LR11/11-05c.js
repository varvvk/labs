const async = require("async");
const rpcClient = require("rpc-websockets").Client;

let ws = new rpcClient("ws://localhost:4000/");

ws.on("open", () => {
  ws.login({ login: "admin", password: "admin" }).then(() => {
    async.waterfall(
      [
        (cb) => {
          ws.call("square", [3])
            .then((r) => {
              console.log("STEP 1 square[3] =", r);
              cb(null, r);
            })
            .catch((err) => {
              console.log("ERROR STEP 1:", err);
              cb(err);
            });
        },

        (sq3, cb) => {
          ws.call("square", [5, 4])
            .then((r) => {
              console.log("STEP 2 square[5,4] =", r);
              cb(null, sq3, r);
            })
            .catch((err) => {
              console.log("ERROR STEP 2:", err);
              cb(err);
            });
        },

        (sq3, sq54, cb) => {
          ws.call("mul", [3, 5, 7, 9, 11, 13])
            .then((r) => {
              console.log("STEP 3 mul =", r);
              cb(null, sq3, sq54, r);
            })
            .catch((err) => {
              console.log("ERROR STEP 3:", err);
              cb(err);
            });
        },

        (sq3, sq54, mul357, cb) => {
          ws.call("sum", [sq3, sq54, mul357])
            .then((r) => {
              console.log("STEP 4 sum =", r);
              cb(null, r);
            })
            .catch((err) => {
              console.log("ERROR STEP 4:", err);
              cb(err);
            });
        },

        (sumResult, cb) => {
          ws.call("fib", [7])
            .then((r) => {
              console.log("STEP 5 fib[7] =", r);
              cb(null, sumResult, r);
            })
            .catch((err) => {
              console.log("ERROR STEP 5:", err);
              cb(err);
            });
        },

        (sumResult, fibArr, cb) => {
          ws.call("mul", [2, 4, 6])
            .then((r) => {
              console.log("STEP 6 mul[2,4,6] =", r);
              cb(null, sumResult, fibArr, r);
            })
            .catch((err) => {
              console.log("ERROR STEP 6:", err);
              cb(err);
            });
        },

        (sumResult, fibArr, mul246, cb) => {
          let result = sumResult + fibArr.slice(-1) * mul246;

          console.log("STEP 7 FINAL RESULT =", result);

          cb(null, result);
        },
      ],

      (err, finalResult) => {
        if (err) {
          console.error("ERROR FINAL:", err);
        } else {
          console.log("FINAL RESULT:", finalResult);
        }
        ws.close();
      }
    );
  });
});