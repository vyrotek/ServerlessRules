# Serverless Rules!

Jason Barnes

[https://aka.ms/serverlesscontest](https://aka.ms/serverlesscontest)

[https://aka.ms/serverlesscontestrules](https://aka.ms/serverlesscontestrules)

---
## Summary

### What?
* Serverless-rules allows you to evaluate externally stored javascript logic.

### How?
* Serverless-rules uses [JINT](https://github.com/sebastienros/jint) to evaluate a JS file from Azure Blob Storage based on the Rule value specified. The data sent in the Input is used as function arguments.

### Why?
* A script file can represent anything. In the examples below each script represents a basic 'rule' that evaluates the input sent to the function through the JS engine and returns a result. A business may use it to evaluate/filter incoming data and decide what to do with it. It could also be used to create "*smart*" mock APIs and much more!
* It's like an *executable* gist or pastebin.

### Next?
* A page that would allow anyone to submit a rule script file.
* Record and display evaluation results.
* Use Azure durable functions to persist data changes between evaluations.
* ??? profit!

---

## Example 1

### Rule Input - From Request
```
curl --location --request POST 'http://serverless-rules.azurewebsites.net/api/RuleEvaluator' \
--header 'Content-Type: application/json' \
--data-raw '{
    "Rule": "basic.js",
    "Input": {
        "a": 10,
        "b": 5
    }
}'
```

### Rule Code - From Blob
[https://serverlessrules.blob.core.windows.net/rules/basic.js](https://serverlessrules.blob.core.windows.net/rules/basic.js)
```
function run(input) {
  const z = (input.a + input.b); 
  return { 
    result : z 
  }; 
}
```

### Rule Result
```
{
    "result": 15
}
```

---

## Example 2

### Rule Input - From Request
```
curl --location --request POST 'http://serverless-rules.azurewebsites.net/api/RuleEvaluator' \
--header 'Content-Type: application/json' \
--data-raw '{
    "Rule": "hiring.js",
    "Input": {
        "name": "Jason Barnes",
        "experience": 15,
        "skills": "C#, SQL, JS",
        "location": "AZ"
    }
}'
```

### Rule Code - From Blob
[https://serverlessrules.blob.core.windows.net/rules/hiring.js](https://serverlessrules.blob.core.windows.net/rules/hiring.js)
```
function run(input) {
	if(input.name === "Jason Barnes") {
		return {
			message: "He's the best!"
		};
	}
	else if(input.skills.includes("C#") &&
	   input.experience > 10 &&
	   input.location === "AZ") {
		return {
			message: "Hire them!"
		};
	}
	else {
		return {
			message: "Pass."
		};
	}
}
```

### Rule Result
```
{
    "message": "He's the best!"
}
```

---
