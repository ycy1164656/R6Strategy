using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parse;

public class ParseInit : ParseInitializeBehaviour {

    public override void Awake()
    {
        //we use a few different URLs, but set CurrentURL and currentID to whatever you want HERE

        ParseInitializeBehaviour nativeParseScript = this.gameObject.AddComponent<ParseInitializeBehaviour>();
        nativeParseScript.applicationID = "hrRpHQ7xt7LzCRJ48MxSgxxL";
        nativeParseScript.dotnetKey = "4vB7L5zUTrOpPU8U94DX2XLe";  //.net key doesnt even matter in Parse Server
        nativeParseScript.serverURL = "http://45.32.255.165:1337/parse/";

        ParseClient.Initialize(new ParseClient.Configuration()
        {
            WindowsKey = "4vB7L5zUTrOpPU8U94DX2XLe", //.net key doesnt even matter in Parse Server
            ApplicationId = "hrRpHQ7xt7LzCRJ48MxSgxxL",
            Server = "http://45.32.255.165:1337/parse/"
        });

        Debug.Log("server is " + ParseClient.CurrentConfiguration.Server); //just checking

    }

}
