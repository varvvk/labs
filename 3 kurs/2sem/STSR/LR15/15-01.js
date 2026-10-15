const { MongoClient } = require('mongodb');
const http = require('http');
const url = require('url');

const uri = "mongodb://KVV:KVV@ac-zkmknnh-shard-00-00.ydpmdmv.mongodb.net:27017,ac-zkmknnh-shard-00-01.ydpmdmv.mongodb.net:27017,ac-zkmknnh-shard-00-02.ydpmdmv.mongodb.net:27017/?ssl=true&replicaSet=atlas-xua8bg-shard-0&authSource=admin&appName=BSTU";

const client = new MongoClient(uri);

let db;

function sendJSON(res, status, data) {
    res.writeHead(status, { "Content-Type": "application/json" });
    res.end(JSON.stringify(data));
}

async function start() {
    try {
        await client.connect();
        console.log("Connected to MongoDB");

        db = client.db("BSTU");

        const server = http.createServer(async (req, res) => {
            const parsedUrl = url.parse(req.url, true);
            const method = req.method;
            const pathname = parsedUrl.pathname;

            if (method === 'GET' && pathname === '/api/faculties') {
                try {
                    const docs = await db.collection("faculty").find().toArray();
                    sendJSON(res, 200, docs);
                } catch (err) {
                    sendJSON(res, 500, { error: "DB error", details: err.message });
                }
                return;
            }

            if (method === 'GET' && pathname === '/api/pulpits') {
                try {
                    const docs = await db.collection("pulpit").find().toArray();
                    sendJSON(res, 200, docs);
                } catch (err) {
                    sendJSON(res, 500, { error: "DB error", details: err.message });
                }
                return;
            }

            if (method === 'POST' && pathname === '/api/faculties') {
                let body = "";
                req.on("data", chunk => body += chunk);

                req.on("end", async () => {
                    try {
                        const data = JSON.parse(body);
                        await db.collection("faculty").insertOne(data);
                        sendJSON(res, 200, data);
                    } catch (err) {
                        sendJSON(res, 500, { error: "DB error", details: err.message });
                    }
                });

                return;
            }

            if (method === 'POST' && pathname === '/api/pulpits') {
                let body = "";
                req.on("data", chunk => body += chunk);

                req.on("end", async () => {
                    try {
                        const data = JSON.parse(body);
                        await db.collection("pulpit").insertOne(data);
                        sendJSON(res, 200, data);
                    } catch (err) {
                        sendJSON(res, 500, { error: "DB error", details: err.message });
                    }
                });

                return;
            }

           if (method === 'PUT' && pathname === '/api/faculties') {
                let body = "";
                req.on("data", chunk => body += chunk);

                req.on("end", async () => {
                    try {
                        const data = JSON.parse(body);

                        const facultyCode = data.faculty?.trim();

                        if (!facultyCode) {
                            return sendJSON(res, 400, {
                                error: "faculty is required"
                            });
                        }

                        const updateResult = await db.collection("faculty").updateOne(
                            { faculty: facultyCode },
                            {
                                $set: {
                                    faculty: facultyCode,
                                    faculty_name: data.faculty_name
                                }
                            }
                        );

                        if (updateResult.matchedCount === 0) {
                            return sendJSON(res, 404, {
                                error: "Faculty not found",
                                searchedValue: facultyCode
                            });
                        }

                        const updatedDoc = await db.collection("faculty").findOne({
                            faculty: facultyCode
                        });

                        return sendJSON(res, 200, {
                            message: "Updated successfully",
                            data: updatedDoc
                        });

                    } catch (err) {
                        return sendJSON(res, 500, {
                            error: "DB error",
                            details: err.message
                        });
                    }
                });

                return;
            }

            if (method === 'PUT' && pathname === '/api/pulpits') {
                let body = "";
                req.on("data", chunk => body += chunk);

                req.on("end", async () => {
                    try {
                        const data = JSON.parse(body);

                        const pulpitCode = data.pulpit?.trim();

                        if (!pulpitCode) {
                            return sendJSON(res, 400, {
                                error: "pulpit is required"
                            });
                        }

                        const updateResult = await db.collection("pulpit").updateOne(
                            { pulpit: pulpitCode },
                            {
                                $set: {
                                    pulpit: pulpitCode,
                                    pulpit_name: data.pulpit_name,
                                    faculty: data.faculty
                                }
                            }
                        );

                        if (updateResult.matchedCount === 0) {
                            return sendJSON(res, 404, {
                                error: "Pulpit not found",
                                searchedValue: pulpitCode
                            });
                        }

                        const updatedDoc = await db.collection("pulpit").findOne({
                            pulpit: pulpitCode
                        });

                        return sendJSON(res, 200, {
                            message: "Updated successfully",
                            data: updatedDoc
                        });

                    } catch (err) {
                        return sendJSON(res, 500, {
                            error: "DB error",
                            details: err.message
                        });
                    }
                });

                return;
            }
            
            if (method === 'DELETE' && pathname.startsWith('/api/faculties/')) {
                let code = decodeURI(pathname.split('/').pop());

                try {
                    const deletedDoc = await db.collection("faculty").findOne({
                        faculty: code
                    });

                    if (!deletedDoc) {
                        return sendJSON(res, 404, {
                            error: "Faculty not found"
                        });
                    }

                    await db.collection("faculty").deleteOne({
                        faculty: code
                    });

                    sendJSON(res, 200, {
                        message: "Deleted successfully",
                        data: deletedDoc
                    });

                } catch (err) {
                    sendJSON(res, 500, {
                        error: "DB error",
                        details: err.message
                    });
                }

                return;
            }

            if (method === 'DELETE' && pathname.startsWith('/api/pulpits/')) {
                let code = decodeURI(pathname.split('/').pop());

                try {
                    const deletedDoc = await db.collection("pulpit").findOne({
                        pulpit: code
                    });

                    if (!deletedDoc) {
                        return sendJSON(res, 404, {
                            error: "Pulpit not found"
                        });
                    }

                    await db.collection("pulpit").deleteOne({
                        pulpit: code
                    });

                    sendJSON(res, 200, {
                        message: "Deleted successfully",
                        data: deletedDoc
                    });

                } catch (err) {
                    sendJSON(res, 500, {
                        error: "DB error",
                        details: err.message
                    });
                }

                return;
            }
        });

        server.listen(3000, () => console.log("Server running on 3000"));

    } catch (err) {
        console.error("Mongo connection error:", err);
    }
}

start();