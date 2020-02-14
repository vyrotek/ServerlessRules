# Serverless Rules!

[aka.ms/serverlesscontest](aka.ms/serverlesscontest)

[aka.ms/serverlesscontestrules](aka.ms/serverlesscontestrules)


## Rule Input
```
curl --location --request POST 'http://serverless-rules.azurewebsites.net/api/HttpEvaluator' \
--header 'Content-Type: application/json' \
--data-raw '{
    "Rule": "basic.js",
    "Input": {
        "a": 10,
        "b": 5
    }
}'
```

## Rule Code (From Blob)
[https://serverlessrules.blob.core.windows.net/rules/basic.js](https://serverlessrules.blob.core.windows.net/rules/basic.js)
```
function run(input) {
  const z = (input.a + input.b); 
  return { 
    result : z 
  }; 
}
```

## Rule Result
```
{
    "result": 15
}
```
