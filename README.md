Oauth 2 Login 
====================================================
	Version 1.2
	
	
This OAuth 2 is a simple login frame with integration of Facebook, Google, Windows Live login function. Just get the user information, the user log in is integrated into application. Support setting server agent configuration as follows:

```  
  <configSections>
    <section name="oauth2.login.configuration" type="Oauth2Login.Configuration.OAuthConfigurationSection, Oauth2Login"/>
  </configSections>
  <oauth2.login.configuration>
    <web acceptedRedirectUrl="~/home/succes" failedRedirectUrl="~/home/error"/>
    <oauth>
     <add name="Google" type="Oauth2Login.Client.GoogleClient, Oauth2Login"
          clientid="1095792391040.apps.googleusercontent.com"
          clientsecret="LsRFXXHr7T26npBJCBAqvjDi"
          callbackUrl="http://github.org/home/succes"
          proxy=""
          scope="https://www.googleapis.com/auth/userinfo.email+https://www.googleapis.com/auth/userinfo.profile"/>
     <add name="Facebook" type="Oauth2Login.Client.FacebookClient, Oauth2Login"
          clientid="	229691003818607"
          clientsecret="a0259390c0b20d6855b39b3edcd14c8a"
          callbackUrl="http://github.org/home/succes"
          proxy="127.0.0.1:8088"
          scope="user_about_me,email,user_photos"/>
     <add name="WindowsLive" type="Oauth2Login.Client.WindowsLiveClient, Oauth2Login"
          clientid="00000000480C9FBC"
          clientsecret="hpxlhyxtmABNXFo5qxuAV6pOfZdsyeZF"
          callbackUrl="http://github.org/home/succes"
          proxy=""
          scope="wl.basic,wl.emails"/>  
	 <add name="PayPal" type="Oauth2Login.Client.PayPalClient, Oauth2Login"
          clientid="AcC2wBD5FPwxscAon02NAgG5lqZYHFtooc3uO0wevS457NPXmCs3jRpBGpPs"
          clientsecret="ENGDvxAjFRORiCel09c-La25ZEU50VEdF-dwOV6Z2mPui_YS41SOTlDmi2Sw"
          callbackUrl="http://github.org/home/succe"
          proxy=""
          scope="profile email address phone"/>
    </oauth>
  </oauth2.login.configuration>
``` 


License
======================================
 
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.<br/><br/>