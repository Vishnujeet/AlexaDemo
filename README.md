# AlexaDemo
Alexa skill with Alexa.Net dll
# Features!
Build alexa skill  
# Technology 
- Visual Studio 2017
- ngrok
- amazon developer account 
# Setup

## Build the alexa skill 
- Step 1 click this link and signup https://developer.amazon.com/
- Step 2 select create skill

![1](https://user-images.githubusercontent.com/42546837/61403428-ee97f980-a8f2-11e9-9903-de05e39a15be.png)

- Step 3 provide the skill name and click create skill

![2](https://user-images.githubusercontent.com/42546837/61403472-00799c80-a8f3-11e9-9b2b-9f0390da267c.png)

- Step 4 Click on json editor

![3](https://user-images.githubusercontent.com/42546837/61403504-0cfdf500-a8f3-11e9-833b-2c8b66bc13e8.png)

- Step 5 Paste the json data

```json
{
    "interactionModel": {
        "languageModel": {
            "invocationName": "perfectlight",
            "intents": [
                {
                    "name": "AMAZON.FallbackIntent",
                    "samples": []
                },
                {
                    "name": "AMAZON.CancelIntent",
                    "samples": []
                },
                {
                    "name": "AMAZON.HelpIntent",
                    "samples": []
                },
                {
                    "name": "AMAZON.StopIntent",
                    "samples": []
                },
                {
                    "name": "AMAZON.NavigateHomeIntent",
                    "samples": []
                },
                {
                    "name": "GetSiteIntent",
                    "slots": [],
                    "samples": [
                        "get list of site"
                    ]
                },
                {
                    "name": "GetFieldIntent",
                    "slots": [
                        {
                            "name": "site",
                            "type": "siteType",
                            "samples": [
                                "Sports Web Site"
                            ]
                        }
                    ],
                    "samples": [
                        "list of connected field",
                        "Get list of field thats connected to {site}"
                    ]
                },
                {
                    "name": "GetFieldStatus",
                    "slots": [
                        {
                            "name": "field",
                            "type": "fieldType",
                            "samples": [
                                "Hocky Field"
                            ]
                        }
                    ],
                    "samples": [
                        "What is the status of {field}"
                    ]
                },
                {
                    "name": "TurnOnPerfectLight",
                    "slots": [
                        {
                            "name": "site",
                            "type": "siteType",
                            "samples": [
                                "Demo site"
                            ]
                        },
                        {
                            "name": "field",
                            "type": "fieldType",
                            "samples": [
                                "hockey field"
                            ]
                        }
                    ],
                    "samples": [
                        "to turn on light",
                        "to turn on light {site} and {field}"
                    ]
                },
                {
                    "name": "TurnOffPerfectLight",
                    "slots": [
                        {
                            "name": "site",
                            "type": "siteType",
                            "samples": [
                                "Demo Site"
                            ]
                        },
                        {
                            "name": "field",
                            "type": "fieldType",
                            "samples": [
                                "Hockey Field"
                            ]
                        }
                    ],
                    "samples": [
                        "turn off Demo light",
                        "turn off Demo light {site} and {field}"
                    ]
                }
            ],
            "types": [
                {
                    "name": "siteType",
                    "values": [
                        {
                            "name": {
                                "value": "Demo Site"
                            }
                        },
                        {
                            "name": {
                                "value": "Sports Web Site"
                            }
                        },
                        {
                            "name": {
                                "value": "ControlAPITestSite"
                            }
                        }
                    ]
                },
                {
                    "name": "fieldType",
                    "values": [
                        {
                            "name": {
                                "value": "Soccer Field"
                            }
                        },
                        {
                            "name": {
                                "value": "Tennis Field"
                            }
                        },
                        {
                            "name": {
                                "value": "Hockey Field"
                            }
                        }
                    ]
                }
            ]
        },
        "dialog": {
            "intents": [
                {
                    "name": "GetFieldIntent",
                    "confirmationRequired": false,
                    "prompts": {},
                    "slots": [
                        {
                            "name": "site",
                            "type": "siteType",
                            "confirmationRequired": false,
                            "elicitationRequired": true,
                            "prompts": {
                                "elicitation": "Elicit.Slot.1237734784040.517253264772"
                            }
                        }
                    ]
                },
                {
                    "name": "GetFieldStatus",
                    "confirmationRequired": false,
                    "prompts": {},
                    "slots": [
                        {
                            "name": "field",
                            "type": "fieldType",
                            "confirmationRequired": false,
                            "elicitationRequired": true,
                            "prompts": {
                                "elicitation": "Elicit.Slot.1000681509922.133310773056"
                            }
                        }
                    ]
                },
                {
                    "name": "TurnOnPerfectLight",
                    "confirmationRequired": false,
                    "prompts": {},
                    "slots": [
                        {
                            "name": "site",
                            "type": "siteType",
                            "confirmationRequired": false,
                            "elicitationRequired": true,
                            "prompts": {
                                "elicitation": "Elicit.Slot.1318141307773.304474914914"
                            }
                        },
                        {
                            "name": "field",
                            "type": "fieldType",
                            "confirmationRequired": false,
                            "elicitationRequired": true,
                            "prompts": {
                                "elicitation": "Elicit.Slot.1318141307773.167332391756"
                            }
                        }
                    ]
                },
                {
                    "name": "TurnOffPerfectLight",
                    "confirmationRequired": false,
                    "prompts": {},
                    "slots": [
                        {
                            "name": "site",
                            "type": "siteType",
                            "confirmationRequired": false,
                            "elicitationRequired": true,
                            "prompts": {
                                "elicitation": "Elicit.Slot.1226062606963.689051044872"
                            }
                        },
                        {
                            "name": "field",
                            "type": "fieldType",
                            "confirmationRequired": false,
                            "elicitationRequired": true,
                            "prompts": {
                                "elicitation": "Elicit.Slot.1226062606963.848863072278"
                            }
                        }
                    ]
                }
            ],
            "delegationStrategy": "ALWAYS"
        },
        "prompts": [
            {
                "id": "Elicit.Slot.1237734784040.517253264772",
                "variations": [
                    {
                        "type": "PlainText",
                        "value": "what is the site name?"
                    }
                ]
            },
            {
                "id": "Elicit.Slot.1000681509922.133310773056",
                "variations": [
                    {
                        "type": "PlainText",
                        "value": "Tell me the field name"
                    }
                ]
            },
            {
                "id": "Elicit.Slot.1318141307773.304474914914",
                "variations": [
                    {
                        "type": "PlainText",
                        "value": "What is the site name?"
                    }
                ]
            },
            {
                "id": "Elicit.Slot.1318141307773.167332391756",
                "variations": [
                    {
                        "type": "PlainText",
                        "value": "What is the field name?"
                    }
                ]
            },
            {
                "id": "Elicit.Slot.1226062606963.689051044872",
                "variations": [
                    {
                        "type": "PlainText",
                        "value": "What is the site name?"
                    }
                ]
            },
            {
                "id": "Elicit.Slot.1226062606963.848863072278",
                "variations": [
                    {
                        "type": "PlainText",
                        "value": "Tell me the field name?"
                    }
                ]
            }
        ]
    }
}
```

- Step 6 Click on save model and build the model 

![4](https://user-images.githubusercontent.com/42546837/61403909-f015f180-a8f3-11e9-98ed-c638864fc34e.png)


- Step 7 click on Test 

![5](https://user-images.githubusercontent.com/42546837/61403932-fefca400-a8f3-11e9-8a63-98edf3a37158.png)


- Step 8 test your skill

![6](https://user-images.githubusercontent.com/42546837/61403969-0e7bed00-a8f4-11e9-9779-7db3f23e272c.png)

## Download the code and setup local environment to communicate with alexa skill
- Step 9 download  ngrok
- Step 10 Download this project and open the solution 
- Step 11 Navigate to solution explorer, right click on dependency and select manage nugetPakeges. Update all dependency
- Step 12 Build the application and run
- Step 13 run the command ‘ngrok http’ and your localhost port.as my api port is 7071 you can check your and update.
- Step 14 copy the forwarding https URL
- Step 15 Go back to https://developer.amazon.com portal open your skill, click on endpoint
    - 1 select end point
    - 2 select https
    - 3 paste the https url that’s generated by ngrok and add /api/alexa as your api url
    - 4 elect higlited option
    - 5 click on save

- Step 16 Run the application and open the alexa test emulator and execute the below command 
It should hit the break point to like it shows down
