version 1.0

<h3>
This is a simple integration of Facebook, Google, Windows Live OAuth 2 login function frame , 
just get the user information, the user is integrated into the application log in,Agent support setting server configuration as follows:
</h3>
<br/>
<code>
  &lt;configSections&gt;
    &lt;section name="oauth2.login.configuration" type="Oauth2Login.Configuration.OAuthConfigurationSection, Oauth2Login"/&gt;
  </configSections>
  <oauth2.login.configuration>
    <web acceptedRedirectUrl="~/home/succes" failedRedirectUrl="~/home/error"/>
    <oauth>
	   <!--<add name="Twitter" type="Oauth2Login.Client.TwitterClient, Oauth2Login"
           clientid="GFXaYzb4PUFhGICi4SSug"
           clientsecret="IflT0JsoURV9BJ1XmEVdjo2Qohu1l71IWNkfYyfQmk"
           callbackUrl="http://github.org/home/succes"
           proxy="192.168.0.50:12000"
           scope="profile"/>-->
      <add name="Google" type="Oauth2Login.Client.GoogleClient, Oauth2Login"
           clientid="1095792391040.apps.googleusercontent.com"
           clientsecret="LsRFXXHr7T26npBJCBAqvjDi"
           callbackUrl="http://github.org/home/succes"
           proxy="192.168.0.50:12000"
           scope="https://www.googleapis.com/auth/userinfo.email+https://www.googleapis.com/auth/userinfo.profile"/>
      <add name="Facebook" type="Oauth2Login.Client.FacebookClient, Oauth2Login"
           clientid="	229691003818607"
           clientsecret="a0259390c0b20d6855b39b3edcd14c8a"
           callbackUrl="http://github.org/home/succes"
           proxy="192.168.0.50:12000"
           scope="user_about_me,email,user_photos"/>
      <add name="WindowsLive" type="Oauth2Login.Client.WindowsLiveClient, Oauth2Login"
           clientid="00000000480C9FBC"
           clientsecret="hpxlhyxtmABNXFo5qxuAV6pOfZdsyeZF"
           callbackUrl="http://github.org/home/succes"
           proxy=""
           scope="wl.basic,wl.emails"/>   
    </oauth>
  </oauth2.login.configuration>
 </code>
 
<br/>
<h3>License</h3><br/>
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at<br/><br/><br/>

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.<br/><br/>