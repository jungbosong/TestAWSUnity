using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

public class StartProgram : MonoBehaviour
{
    DynamoDBContext context;
    AmazonDynamoDBClient DBClient;
    CognitoAWSCredentials credentials;
    private void Awake()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        credentials = new CognitoAWSCredentials("us-east-2:adca83bc-0167-4443-97c2-77f8789b25ad", RegionEndpoint.USEast2);
        DBClient = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast2);
        context = new DynamoDBContext(DBClient);
        CreateUser();
        FindLevel();
    }

    [DynamoDBTable("user_info")]
    public class User
    {
        [DynamoDBHashKey]
        public string ID {get;set;}
        public string name {get;set;}
        public int level {get;set;}
    }

    private void CreateUser()
    {
        User u1 = new User
        {
            ID = "happy",
            name = "happy",
            level = 1111,
        };
        context.SaveAsync(u1, (result) =>
        {
            if(result.Exception != null)
                Debug.Log("Success!!");
            else
                Debug.Log(result.Exception);
        });
    }

    public void FindLevel()
    {
        User u;
        context.LoadAsync<User>("abcd", (AmazonDynamoDBResult<User> result) =>
        {
            if(result.Exception != null)
            {
                Debug.LogException(result.Exception);
                return;
            }
            u = result.Result;
            Debug.Log(u.level);
        }, null);
    }
}
