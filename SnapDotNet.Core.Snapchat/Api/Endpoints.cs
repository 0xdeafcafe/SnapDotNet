using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using SnapDotNet.Core.Miscellaneous.Crypto;
using SnapDotNet.Core.Miscellaneous.Helpers.Async;
using SnapDotNet.Core.Snapchat.Api.Exceptions;
using SnapDotNet.Core.Snapchat.Helpers;
using SnapDotNet.Core.Snapchat.Models;
using SnapDotNet.Core.Snapchat.Models.New;

namespace SnapDotNet.Core.Snapchat.Api
{
	public class Endpoints
	{
		private readonly SnapchatManager _snapchatManager;
		private readonly WebConnect _webConnect;
		private readonly WebConnect _loqWebConnect;

		#region /bq/ Endpoints (use _webConnect)

		private const string BestsEndpointUrl =				"bests";
		private const string SnapBlobEndpointUrl =			"blob";
		private const string ChatMediaEndpointUrl =			"chat_media";
		private const string ClearFeedEndpointUrl =			"clear";
		private const string FindFriendEndpointUrl =		"find_friends";
		private const string FriendEndpointUrl =			"friend";
		private const string LoginEndpointUrl =				"login";
		private const string LogoutEndpointUrl =			"logout";
		private const string StoriesEndpointUrl =			"stories";
		private const string UpdatesEndpointUrl =			"updates";
		private const string BestFriendCountEndpointUrl =	"set_num_best_friends";
		private const string SettingsEndpointUrl =			"settings";
		private const string UpdatesFeaturesEndpointUrl =	"update_feature_settings";
		private const string SendSnapEventsEndpointUrl =	"update_snaps";
		private const string SendStoryEventsEndpointUrl =	"update_stories";
		private const string RegisterEndpointUrl =			"register";
		private const string GetCaptchaEndpointUrl =		"get_captcha";
		private const string SolveCaptchaEndpointUrl =		"solve_captcha";
		private const string RegisterUsernameEndpointUrl =	"register_username";

		#endregion

		#region /loq/ Endpoionts (use _loqWebConnect)

		private const string AllUpdatesEndpointUrl =		"all_updates";

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="snapchatManager"></param>
		/// <returns></returns>
		public Endpoints(SnapchatManager snapchatManager)
		{
			_snapchatManager = snapchatManager;
			_webConnect = new WebConnect(Settings.ApiBasePoint);
			_loqWebConnect = new WebConnect(Settings.ApiLoqBasePoint);
		}

		#region Old Api Functions

		//#region Registration

		///// <summary>
		///// 
		///// </summary>
		///// <param name="age"></param>
		///// <param name="birthday"></param>
		///// <param name="email"></param>
		///// <param name="password"></param>
		///// <returns></returns>
		//public async Task<Captcha> RegisterAndGetCaptchaAsync(int age, string birthday, string email, string password)
		//{
		//	var registration = await RegisterAsync(age, birthday, email, password);
		//	return await GetCaptchaImagesAsync(registration.Email, registration.AuthToken);
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="age"></param>
		///// <param name="birthday"></param>
		///// <param name="email"></param>
		///// <param name="password"></param>
		///// <returns></returns>
		//public Captcha RegisterAndGetCaptcha(int age, string birthday, string email, string password)
		//{
		//	return RegisterAndGetCaptchaAsync(age, birthday, email, password).Result;
		//}

		//#region Step 1: Registering

		///// <summary>
		///// 
		///// </summary>
		///// <param name="age"></param>
		///// <param name="birthday"></param>
		///// <param name="email"></param>
		///// <param name="password"></param>
		///// <returns></returns>
		//public async Task<RegistrationResponse> RegisterAsync(int age, string birthday, string email, string password)
		//{
		//	var timestamp = Timestamps.GenerateRetardedTimestamp();
		//	var postData = new Dictionary<string, string>
		//	{
		//		{"age",age.ToString()},
		//		{"birthday", birthday}, // YYYY-MM-DD
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
		//		{"email", email},
		//		{"password", password}
		//	};

		//	var response = 
		//		await
		//			_webConnect.PostToGenericAsync<RegistrationResponse>(RegisterEndpointUrl, postData, Settings.StaticToken,
		//				timestamp.ToString(CultureInfo.InvariantCulture));

		//	if (!response.Logged)
		//		throw new InvalidRegistrationException(response.Message);

		//	return response;
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="age"></param>
		///// <param name="birthday"></param>
		///// <param name="email"></param>
		///// <param name="password"></param>
		///// <returns></returns>
		//public RegistrationResponse Register(int age, string birthday, string email, string password)
		//{
		//	return RegisterAsync(age, birthday, email, password).Result;
		//}

		//#endregion

		//#region Step 2: Getting Captchas

		///// <summary>
		///// 
		///// </summary>
		///// <param name="email"></param>
		///// <param name="authToken"></param>
		///// <returns></returns>
		//public async Task<Captcha> GetCaptchaImagesAsync(string email, string authToken)
		//{
		//	var timestamp = Timestamps.GenerateRetardedTimestamp();
		//	var postData = new Dictionary<string, string>
		//	{
		//		{"username", email},
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}
		//	};

		//	var response =
		//		await
		//			_webConnect.PostToResponseAsync(GetCaptchaEndpointUrl, postData, authToken,
		//				timestamp.ToString(CultureInfo.InvariantCulture));

		//	var captchaId = response.Content.Headers.ToString();
		//	captchaId = captchaId.Substring(captchaId.IndexOf("filename=") + 9);
		//	captchaId = captchaId.Substring(0, captchaId.IndexOf(".zip\r\n"));

		//	var files = await Zip.ExtractAllFilesAsync(await response.Content.ReadAsByteArrayAsync());

		//	return new Captcha(captchaId, files);
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="email"></param>
		///// <param name="authToken"></param>
		///// <returns></returns>
		//public Captcha GetCaptchaImages(string email, string authToken)
		//{
		//	return GetCaptchaImagesAsync(email, authToken).Result;
		//}

		//#endregion

		//#region Step 3: Submitting Captcha Solution

		///// <summary>
		///// 
		///// </summary>
		///// <param name="email"></param>
		///// <param name="captchaId"></param>
		///// <param name="authToken"></param>
		///// <param name="captchaImagesWithGhosts"></param>
		///// <returns></returns>
		//public async Task<bool> SolveCaptchaAsync(string email, string captchaId, string authToken, bool[] captchaImagesWithGhosts)
		//{
		//	var timestamp = Timestamps.GenerateRetardedTimestamp();
		//	var postData = new Dictionary<string, string>
		//	{
		//		{"captcha_solution", captchaImagesWithGhosts.Aggregate("", (current, b) => current + (b ? "1" : "0"))},
		//		{"captcha_id", captchaId},
		//		{"username", email},
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
		//	};

		//	try
		//	{
		//		// Http request went through, meaning the captcha was solved correctly
		//		await
		//			_webConnect.PostToResponseAsync(SolveCaptchaEndpointUrl, postData, authToken,
		//				timestamp.ToString(CultureInfo.InvariantCulture));

		//		return true;
		//	}

		//	catch (InvalidHttpResponseException ex)
		//	{
		//		switch (ex.HttpResponseMessage.StatusCode)
		//		{
		//			// This is given when the captcha is solved incorrectly.
		//			case HttpStatusCode.Forbidden:
		//				return false;

		//			default:
		//				throw;
		//		}
		//	}
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="email"></param>
		///// <param name="captchaId"></param>
		///// <param name="authToken"></param>
		///// <param name="captchaImagesWithGhosts"></param>
		///// <returns></returns>
		//public bool SolveCaptcha(string email, string captchaId, string authToken, bool[] captchaImagesWithGhosts)
		//{
		//	return SolveCaptchaAsync(email, captchaId, authToken, captchaImagesWithGhosts).Result;
		//}

		//#endregion

		//#region Step 4: Attaching a Username

		///// <summary>
		///// 
		///// </summary>
		///// <param name="email"></param>
		///// <param name="authToken"></param>
		///// <param name="requestedUsername"></param>
		///// <returns></returns>
		//public async Task<Account> RegisterUsernameAsync(string email, string authToken, string requestedUsername)
		//{
		//	// Currently broken (returns 401)

		//	var timestamp = Timestamps.GenerateRetardedTimestamp();
		//	var postData = new Dictionary<string, string>
		//	{
		//		{"selected_username", requestedUsername},
		//		{"username", email},
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
		//	};

		//	var account =
		//		await
		//			_webConnect.PostToGenericAsync<Account>(RegisterUsernameEndpointUrl, postData, Settings.StaticToken,
		//				timestamp.ToString(CultureInfo.InvariantCulture));

		//	//if (account == null || !account.Logged)
		//	//	throw new InvalidCredentialsException();

		//	_snapchatManager.UpdateAccount(account);
		//	_snapchatManager.UpdateAuthToken(account.AuthToken);
		//	_snapchatManager.UpdateUsername(account.Username);
		//	_snapchatManager.Save();

		//	return account;
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="email"></param>
		///// <param name="authToken"></param>
		///// <param name="requestedUsername"></param>
		///// <returns></returns>
		//public Account RegisterUsername(string email, string authToken, string requestedUsername)
		//{
		//	return RegisterUsernameAsync(email, authToken, requestedUsername).Result;
		//}

		//#endregion

		//#endregion

		//#region Authenticate

		///// <summary>
		///// 
		///// </summary>
		///// <param name="username"></param>
		///// <param name="password"></param>
		///// <returns></returns>
		//public async Task<Account> AuthenticateAsync(string username, string password)
		//{
		//	var timestamp = Timestamps.GenerateRetardedTimestamp();
		//	var postData = new Dictionary<string, string>
		//	{
		//		{"password", password},
		//		{"username", username},
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}
		//	};

		//	var account =
		//		await
		//			_webConnect.PostToGenericAsync<Account>(LoginEndpointUrl, postData, Settings.StaticToken,
		//				timestamp.ToString(CultureInfo.InvariantCulture));

		//	if (account == null || !account.Logged)
		//		throw new InvalidCredentialsException();

		//	_snapchatManager.UpdateAccount(account);
		//	_snapchatManager.UpdateAuthToken(account.AuthToken);
		//	_snapchatManager.UpdateUsername(account.Username);
		//	_snapchatManager.Save();

		//	return account;
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="username"></param>
		///// <param name="password"></param>
		///// <returns></returns>
		//public Account Authenticate(string username, string password)
		//{
		//	return AuthenticateAsync(username, password).Result;
		//}

		//#endregion

		//#region Logout

		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//public async Task<Response> LogoutAsync()
		//{
		//	await _snapchatManager.DeleteAsync();

		//	var timestamp = Timestamps.GenerateRetardedTimestamp();
		//	var postData = new Dictionary<string, string>
		//	{
		//		{"username", GetAuthedUsername()},
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
		//		{"json", "{}"}
		//	};

		//	var response =
		//		await
		//			_webConnect.PostToGenericAsync<Response>(LogoutEndpointUrl, postData, _snapchatManager.AuthToken,
		//				timestamp.ToString(CultureInfo.InvariantCulture));

		//	return response;
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//public Response Logout()
		//{
		//	return LogoutAsync().Result;
		//}

		//#endregion

		//#region Updates

		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//public async Task<Account> GetUpdatesAsync()
		//{
		//	var timestamp = Timestamps.GenerateRetardedTimestamp();
		//	var postData = new Dictionary<string, string>
		//	{
		//		{"username", GetAuthedUsername()},
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}
		//	};

		//	var account =
		//		await
		//			_webConnect.PostToGenericAsync<Account>(UpdatesEndpointUrl, postData, _snapchatManager.AuthToken,
		//				timestamp.ToString(CultureInfo.InvariantCulture));

		//	if (account == null || !account.Logged)
		//		throw new InvalidCredentialsException();

		//	_snapchatManager.UpdateAccount(account);
		//	_snapchatManager.UpdateAuthToken(account.AuthToken);
		//	_snapchatManager.UpdateUsername(account.Username);
		//	_snapchatManager.Save();

		//	return account;
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//public Account GetUpdates()
		//{
		//	return GetUpdatesAsync().Result;
		//}

		//#endregion

		//#region Stories

		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//public async Task<Stories> GetStoriesAsync()
		//{
		//	var timestamp = Timestamps.GenerateRetardedTimestamp();
		//	var postData = new Dictionary<string, string>
		//	{
		//		{"username", GetAuthedUsername()},
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}
		//	};

		//	var stories =
		//		await
		//			_webConnect.PostToGenericAsync<Stories>(StoriesEndpointUrl, postData, _snapchatManager.AuthToken,
		//				timestamp.ToString(CultureInfo.InvariantCulture));

		//	_snapchatManager.UpdateStories(stories);

		//	return stories;
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//public Stories GetStories()
		//{
		//	return GetStoriesAsync().Result;
		//}

		//#endregion

		//#region Snap Events

		///// <summary>
		///// </summary>
		///// <param name="snapId"></param>
		///// <param name="timeViewed"></param>
		///// <param name="captureTime"></param>
		///// <returns></returns>
		//public async Task<bool> SendSnapViewedEventAsync(string snapId, int timeViewed, int captureTime)
		//{
		//	var snapInfo = new Dictionary<string, Dictionary<string, double>>
		//	{
		//		{
		//			snapId,
		//			new Dictionary<string, double>
		//			{
		//				{"t", Timestamps.GenerateRetardedTimestampWithMilliseconds()},
		//				{"sv", captureTime + new Random(0xdead).NextDouble()}
		//			}
		//		}
		//	};

		//	var events = new[]
		//	{
		//		Events.CreateEvent(Events.EventType.SnapViewed, snapId, timeViewed),
		//		Events.CreateEvent(Events.EventType.SnapExpired, snapId, timeViewed + captureTime)
		//	};

		//	return await SendSnapEventsAsync(events, snapInfo);
		//}

		///// <summary>
		///// </summary>
		///// <param name="snapId"></param>
		///// <param name="timeViewed"></param>
		///// <param name="captureTime"></param>
		///// <returns></returns>
		//public bool SendSnapViewedEvent(string snapId, int timeViewed, int captureTime)
		//{
		//	return SendSnapViewedEventAsync(snapId, timeViewed, captureTime).Result;
		//}

		///// <summary>
		///// </summary>
		///// <param name="events"></param>
		///// <param name="snapInfo"></param>
		///// <returns></returns>
		//private async Task<bool> SendSnapEventsAsync(Dictionary<string, object>[] events, Dictionary<string, Dictionary<string, double>> snapInfo)
		//{
		//	var timestamp = Timestamps.GenerateRetardedTimestamp();
		//	var postData = new Dictionary<string, string>
		//	{
		//		{"username", GetAuthedUsername()},
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
		//		{"events", JsonConvert.SerializeObject(events)},
		//		{"json", JsonConvert.SerializeObject(snapInfo)}
		//	};


		//	await
		//		_webConnect.PostToStringAsync(SendSnapEventsEndpointUrl, postData, _snapchatManager.AuthToken,
		//			timestamp.ToString(CultureInfo.InvariantCulture));

		//	return true;
		//}

		//#endregion

		//#region Story Events

		///// <summary>
		///// </summary>
		///// <param name="storyId"></param>
		///// <param name="screenshotCount"></param>
		///// <returns></returns>
		//public async Task<bool> SendStoryViewedEventAsync(string storyId, int screenshotCount = 0)
		//{
		//	var timestamp = Timestamps.GenerateRetardedTimestampWithMilliseconds();
		//	var storyInfo =
		//		new Dictionary<string, string>
		//		{
		//			{"id", storyId},
		//			{"screenshot_count", screenshotCount.ToString()},
		//			{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}


		//		};

		//	return await SendStoryEventsAsync(storyInfo);
		//}

		///// <summary>
		///// </summary>
		///// <param name="storyId"></param>
		///// <param name="screenshotCount"></param>
		///// <returns></returns>
		//public bool SendStoryViewedEvent(string storyId, int screenshotCount = 0)
		//{
		//	return SendStoryViewedEventAsync(storyId, screenshotCount).Result;
		//}

		///// <summary>
		///// </summary>
		///// <param name="storyInfo"></param>
		///// <returns></returns>
		//private async Task<bool> SendStoryEventsAsync(Dictionary<string, string> storyInfo)
		//{
		//	var timestamp = Timestamps.GenerateRetardedTimestamp();
		//	var postData = new Dictionary<string, string>
		//	{
		//		{"username", GetAuthedUsername()},
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
		//		{"friend_stories", JsonConvert.SerializeObject(storyInfo)},
		//	};

		//	await
		//		_webConnect.PostToStringAsync(SendStoryEventsEndpointUrl, postData, _snapchatManager.AuthToken,
		//			timestamp.ToString(CultureInfo.InvariantCulture));

		//	return true;
		//}

		//#endregion

		//#region Friend Actions

		///// <summary>
		///// 
		///// </summary>
		///// <param name="friendUsername"></param>
		///// <param name="action"></param>
		///// <returns></returns>
		//public async Task<Response> SendFriendActionAsync(string friendUsername, FriendAction action)
		//{
		//	var timestamp = Timestamps.GenerateRetardedTimestamp();
		//	var postData = new Dictionary<string, string>
		//	{
		//		{"username", GetAuthedUsername()},
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
		//		{"action", action.ToString().ToLower()},
		//		{"friend", friendUsername},
		//	};

		//	var response =
		//		await
		//			_webConnect.PostToGenericAsync<Response>(FriendEndpointUrl, postData, _snapchatManager.AuthToken,
		//				timestamp.ToString(CultureInfo.InvariantCulture));

		//	return response;
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="friendUsername"></param>
		///// <param name="action"></param>
		///// <returns></returns>
		//public Response SendFriendAction(string friendUsername, FriendAction action)
		//{
		//	return SendFriendActionAsync(friendUsername, action).Result;
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="friendUsername"></param>
		///// <param name="newDisplayName"></param>
		///// <returns></returns>
		//public async Task<Response> ChangeFriendDisplayNameAsync(string friendUsername, string newDisplayName)
		//{
		//	var timestamp = Timestamps.GenerateRetardedTimestamp();
		//	var postData = new Dictionary<string, string>
		//	{
		//		{"username", GetAuthedUsername()},
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
		//		{"action", "display"},
		//		{"friend", friendUsername},
		//		{"display", newDisplayName}
		//	};

		//	var response =
		//		await
		//			_webConnect.PostToGenericAsync<Response>(FriendEndpointUrl, postData, _snapchatManager.AuthToken,
		//				timestamp.ToString(CultureInfo.InvariantCulture));

		//	return response;
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="friendUsername"></param>
		///// <param name="newDisplayName"></param>
		///// <returns></returns>
		//public Response ChangeFriendDisplayName(string friendUsername, string newDisplayName)
		//{
		//	return ChangeFriendDisplayNameAsync(friendUsername, newDisplayName).Result;
		//}

		//#endregion

		//#region Update Settings

		//#region Settings

		//#region Update Birthday

		///// <summary>
		///// </summary>
		///// <param name="birthMonth"></param>
		///// <param name="birthDay"></param>
		///// <returns></returns>
		//public async Task<bool> UpdateBirthdayAsync(int birthMonth, int birthDay)
		//{
		//	return await UpdateSettingAsync("updateBirthday", new Dictionary<string, string> { { "birthday", string.Format("{0}-{1}", birthMonth, birthDay) } });
		//}

		///// <summary>
		///// </summary>
		///// <param name="birthMonth"></param>
		///// <param name="birthDay"></param>
		///// <returns></returns>
		//public bool UpdateBirthday(int birthMonth, int birthDay)
		//{
		//	return UpdateBirthdayAsync(birthMonth, birthDay).Result;
		//}

		//#endregion

		//#region Update Email

		///// <summary>
		///// </summary>
		///// <param name="email"></param>
		///// <returns></returns>
		//public async Task<bool> UpdateEmailAsync(string email)
		//{
		//	return await UpdateSettingAsync("updateEmail", new Dictionary<string, string> {{"email", email}});
		//}

		///// <summary>
		///// </summary>
		///// <param name="email"></param>
		///// <returns></returns>
		//public bool UpdateEmail(string email)
		//{
		//	return UpdateEmailAsync(email).Result;
		//}

		//#endregion

		//#region Update Account Privacy

		///// <summary>
		///// </summary>
		///// <param name="isPrivate"></param>
		///// <returns></returns>
		//public async Task<bool> UpdateAccountPrivacyAsync(bool isPrivate)
		//{
		//	return await UpdateSettingAsync("updatePrivacy", new Dictionary<string, string> {{"privacySetting", isPrivate ? "1" : "0"}});
		//}

		///// <summary>
		///// </summary>
		///// <param name="isPrivate"></param>
		///// <returns></returns>
		//public bool UpdateAccountPrivacy(bool isPrivate)
		//{
		//	return UpdateAccountPrivacyAsync(isPrivate).Result;
		//}

		//#endregion

		//#region Update Story Privacy

		///// <summary>
		///// </summary>
		///// <param name="friendsOnly"></param>
		///// <param name="friendsToBlock"></param>
		///// <returns></returns>
		//public async Task<bool> UpdateStoryPrivacyAsync(bool friendsOnly, List<string> friendsToBlock = null)
		//{
		//	var privacySetting = !friendsOnly ? "EVERYONE" : (friendsToBlock == null) ? "FRIENDS" : "CUSTOM";

		//	var extraPostData = new Dictionary<string, string> {{"privacySetting", privacySetting}};

		//	if (friendsToBlock == null)
		//		return await UpdateSettingAsync("updateStoryPrivacy", extraPostData);

		//	var blockedFriendsData = "";
		//	foreach (var s in friendsToBlock)
		//	{
		//		blockedFriendsData += string.Format("'{0}'", s);

		//		if (friendsToBlock.IndexOf(s) != friendsToBlock.Count - 1)
		//			blockedFriendsData += ",";
		//	}
		//	extraPostData.Add("storyFriendsToBlock", string.Format("[{0}]", blockedFriendsData));

		//	return await UpdateSettingAsync("updateStoryPrivacy", extraPostData);
		//}

		///// <summary>
		///// </summary>
		///// <param name="friendsOnly"></param>
		///// <param name="friendsToBlock"></param>
		///// <returns></returns>
		//public bool UpdateStoryPrivacy(bool friendsOnly, List<string> friendsToBlock = null)
		//{
		//	return UpdateStoryPrivacyAsync(friendsOnly, friendsToBlock).Result;
		//}

		//#endregion

		//#region Update Maturity Settings

		///// <summary>
		///// </summary>
		///// <param name="canViewMatureContent"></param>
		///// <returns></returns>
		//public async Task<bool> UpdateMaturitySettingsAsync(bool canViewMatureContent)
		//{
		//	return await UpdateSettingAsync("updateCanViewMatureContent", new Dictionary<string, string> {{"canViewMatureContent", canViewMatureContent.ToString()}});
		//}

		///// <summary>
		///// </summary>
		///// <param name="canViewMatureContent"></param>
		///// <returns></returns>
		//public bool UpdateMaturitySettings(bool canViewMatureContent)
		//{
		//	return UpdateMaturitySettingsAsync(canViewMatureContent).Result;
		//}

		//#endregion

		//#region Update Base

		///// <summary>
		///// 
		///// </summary>
		///// <param name="actionName"></param>
		///// <param name="extraPostData"></param>
		///// <returns></returns>
		//private async Task<bool> UpdateSettingAsync(string actionName, Dictionary<string, string> extraPostData = null)
		//{
		//	var timestamp = Timestamps.GenerateRetardedTimestamp();
		//	var postData = new Dictionary<string, string>
		//	{
		//		{"username", GetAuthedUsername()},
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
		//		{"action", actionName},
		//	};

		//	if (extraPostData != null)
		//		foreach (var postDataEntry in extraPostData)
		//			postData.Add(postDataEntry.Key, postDataEntry.Value);

		//	var response =
		//		await
		//			_webConnect.PostToGenericAsync<Response>(SettingsEndpointUrl, postData, _snapchatManager.AuthToken,
		//				timestamp.ToString(CultureInfo.InvariantCulture));

		//	if (actionName != "updateCanViewMatureContent" && !response.Logged)
		//		throw new InvalidCredentialsException();

		//	return true;
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="actionName"></param>
		///// <param name="extraPostData"></param>
		///// <returns></returns>
		//public bool UpdateSetting(string actionName, Dictionary<string, string> extraPostData = null)
		//{
		//	return UpdateSettingAsync(actionName, extraPostData).Result;
		//}

		//#endregion

		//#endregion

		//#region Feature Settings

		///// <summary>
		///// </summary>
		///// <param name="smartFilters"></param>
		///// <param name="visualFilters"></param>
		///// <param name="specialText"></param>
		///// <param name="replaySnaps"></param>
		///// <param name="frontFacingFlash"></param>
		///// <returns></returns>
		//public async Task<bool> UpdateFeatureSettingsAsync(bool smartFilters = false, bool visualFilters = false, bool specialText = false, bool replaySnaps = false, bool frontFacingFlash = false)
		//{

		//	var timestamp = Timestamps.GenerateRetardedTimestamp();
		//	var postData = new Dictionary<string, string>
		//	{
		//		{"username", GetAuthedUsername()},
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
		//		{"settings", "{" + string.Format("\"smart_filters\": {0}, \"visual_filters\": {1}, \"special_text\": {2}, \"replay_snaps\": {3}, \"front_facing_flash\": {4}", smartFilters, visualFilters, specialText, replaySnaps, frontFacingFlash) + "}"}
		//	};

		//	await
		//		_webConnect.PostToGenericAsync<Response>(UpdatesFeaturesEndpointUrl, postData, _snapchatManager.AuthToken,
		//			timestamp.ToString(CultureInfo.InvariantCulture));

		//	return true;
		//}

		///// <summary>
		///// </summary>
		///// <param name="smartFilters"></param>
		///// <param name="visualFilters"></param>
		///// <param name="specialText"></param>
		///// <param name="replaySnaps"></param>
		///// <param name="frontFacingFlash"></param>
		///// <returns></returns>
		//public bool UpdateFeatureSettings(bool smartFilters = false, bool visualFilters = false, bool specialText = false, bool replaySnaps = false, bool frontFacingFlash = false)
		//{
		//	return UpdateFeatureSettingsAsync(smartFilters, visualFilters, specialText, replaySnaps, frontFacingFlash).Result;
		//}

		//#endregion

		//#endregion

		//#region Public Activity

		///// <summary>
		///// 
		///// </summary>
		///// <param name="requestedUsernames"></param>
		///// <returns></returns>
		//public async Task<ObservableDictionary<string, PublicActivity>> GetPublicActivityAsync(string[] requestedUsernames = null)
		//{
		//	if (requestedUsernames == null)
		//		requestedUsernames = _snapchatManager.Account.Friends.Select(friend => friend.Name).ToArray();

		//	var timestamp = Timestamps.GenerateRetardedTimestamp();
		//	var postData = new Dictionary<string, string>
		//	{
		//		{"username", GetAuthedUsername()},
		//		{"friend_usernames", JsonConvert.SerializeObject(requestedUsernames)},
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}
		//	};

		//	var publicActivities =
		//		await
		//			_webConnect.PostToGenericAsync<ObservableDictionary<string, PublicActivity>>(BestsEndpointUrl, postData, _snapchatManager.AuthToken,
		//				timestamp.ToString(CultureInfo.InvariantCulture));

		//	_snapchatManager.UpdatePublicActivities(publicActivities);
		//	_snapchatManager.Save();

		//	return publicActivities;
		//}

		//public ObservableDictionary<string, PublicActivity> GetPublicActivity(string[] requestedUsernames = null)
		//{
		//	return GetPublicActivityAsync(requestedUsernames).Result;
		//}

		//#endregion

		//#region Find Friends

		///// <summary>
		///// 
		///// </summary>
		///// <param name="countryCode"></param>
		///// <param name="numberAndDisplayNameCollection">A collection of Tuples, where the first item is a phone full number
		///// (without country code), and the second is the Display Name that will be applied to the account with this number.</param>
		///// <returns></returns>
		//public async Task<FoundFriend> FindFriendsAsync(string countryCode, ObservableCollection<Tuple<string, string>> numberAndDisplayNameCollection)
		//{
		//	// Currently broken (returns 400)

		//	// There has to be a better way...
		//	var numberString = "{";
		//	foreach (var t in numberAndDisplayNameCollection)
		//	{
		//		numberString += string.Format("\"{0}\": \"{1}\"", t.Item1, t.Item2);
		//		if (numberAndDisplayNameCollection.IndexOf(t) < numberAndDisplayNameCollection.Count - 1)
		//			numberString += ",";
		//	}
		//	numberString += "}";

		//	var timestamp = Timestamps.GenerateRetardedTimestamp();
		//	var postData = new Dictionary<string, string>
		//	{
		//		{"username", GetAuthedUsername()},
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
		//		{"countryCode", countryCode},
		//		{"numbers", numberString}
		//	};

		//	return 
		//		await
		//			_webConnect.PostToGenericAsync<FoundFriend>(FindFriendEndpointUrl, postData, _snapchatManager.AuthToken,
		//				timestamp.ToString(CultureInfo.InvariantCulture));
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="countryCode"></param>
		///// <param name="numberAndDisplayNameCollection">A collection of Tuples, where the first item is a phone full number
		///// (without country code), and the second is the Display Name that will be applied to the account with this number.</param>
		///// <returns></returns>
		//public FoundFriend FindFriends(string countryCode, ObservableCollection<Tuple<string, string>> numberAndDisplayNameCollection)
		//{
		//	return FindFriendsAsync(countryCode, numberAndDisplayNameCollection).Result;
		//}

		//#endregion

		//#region Clear Feed

		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//public async Task<bool> ClearFeedAsync()
		//{

		//	var timestamp = Timestamps.GenerateRetardedTimestamp();
		//	var postData = new Dictionary<string, string>
		//	{
		//		{"username", GetAuthedUsername()},
		//		{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}
		//	};

		//	var response =
		//		await
		//			_webConnect.PostToResponseAsync(ClearFeedEndpointUrl, postData, _snapchatManager.AuthToken,
		//				timestamp.ToString(CultureInfo.InvariantCulture));

		//	if (response.StatusCode == HttpStatusCode.OK)
		//		return true;

		//	throw new InvalidHttpResponseException(response.ReasonPhrase, response);
		//}

		//public bool ClearFeed()
		//{
		//	return ClearFeedAsync().Result;
		//}

		//#endregion

		#endregion

		#region New Api Functions

		#region Authentication

		/// <summary>
		/// 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public async Task<Response> AuthenticateAsync(string username, string password)
		{
			var timestamp = Timestamps.GenerateRetardedTimestamp();
			var postData = new Dictionary<string, string>
			{
				{"password", password},
				{"username", username},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}
			};

			var account =
				await
					_webConnect.PostToGenericAsync<Response>(LoginEndpointUrl, postData, Settings.StaticToken,
						timestamp.ToString(CultureInfo.InvariantCulture));

			if (account == null || !account.Logged)
				throw new InvalidCredentialsException();

			_snapchatManager.Save();

			return account;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public Response Authenticate(string username, string password)
		{
			return AsyncHelpers.RunSync(() => AuthenticateAsync(username, password));
		}

		#endregion

		#region Logout

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<Response> LogoutAsync()
		{
			await _snapchatManager.DeleteAsync();

			var timestamp = Timestamps.GenerateRetardedTimestamp();
			var postData = new Dictionary<string, string>
			{
				{"username", GetAuthedUsername()},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
				{"json", "{}"}
			};

			var response =
				await
					_webConnect.PostToGenericAsync<Response>(LogoutEndpointUrl, postData, GetAuthToken(),
						timestamp.ToString(CultureInfo.InvariantCulture));

			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Response Logout()
		{
			return AsyncHelpers.RunSync(LogoutAsync);
		}

		#endregion

		#region Updates

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public async Task<AllUpdatesResponse> GetAllUpdatesAsync()
		{
			var timestamp = Timestamps.GenerateRetardedTimestamp();
			var postData = new Dictionary<string, string>
			{
				{"username", GetAuthedUsername()},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}
			};

			var allUpdatesResponse =
				await
					_loqWebConnect.PostToGenericAsync<AllUpdatesResponse>(AllUpdatesEndpointUrl, postData, GetAuthToken(),
						timestamp.ToString(CultureInfo.InvariantCulture));

			if (allUpdatesResponse == null || allUpdatesResponse.UpdatesResponse == null || !allUpdatesResponse.UpdatesResponse.Logged)
				throw new InvalidCredentialsException();

			_snapchatManager.Update(allUpdatesResponse);
			_snapchatManager.Save();

			return allUpdatesResponse;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public AllUpdatesResponse GetAllUpdates()
		{
			return AsyncHelpers.RunSync(GetAllUpdatesAsync);
		}

		#endregion

		#region Settings

		#region Set Number of BFFs

		/// <summary>
		/// 
		/// </summary>
		/// <param name="newBestFriendCount"> Must be 3, 5, or 7.</param>
		/// <returns></returns>
		public async Task<BestFriends> SetBestFriendCountAsync(int newBestFriendCount)
		{
			if (newBestFriendCount != 3 &&
				newBestFriendCount != 5 &&
				newBestFriendCount != 7)
				throw new InvalidParameterException("numberOfBestFriends must be 3, 5, or 7.");

			var timestamp = Timestamps.GenerateRetardedTimestamp();
			var postData = new Dictionary<string, string>
			{
				{"username", GetAuthedUsername()},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
				{"num_best_friends", newBestFriendCount.ToString()},
			};

			var response =
				await
					_webConnect.PostToGenericAsync<BestFriends>(BestFriendCountEndpointUrl, postData, _snapchatManager.AuthToken,
						timestamp.ToString(CultureInfo.InvariantCulture));

			return response;
		}

		public BestFriends SetBestFriendCount(int numberOfBestFriends)
		{
			return AsyncHelpers.RunSync(() => SetBestFriendCountAsync(numberOfBestFriends));
		}

		#endregion

		#endregion

		#region Blob

		///// <summary>
		///// 
		///// </summary>
		///// <param name="storyInstance"></param>
		///// <returns></returns>
		//public byte[] GetStoryThumbnailBlob(Story storyInstance)
		//{
		//	return GetStoryBlob(new Uri(storyInstance.ThumbnailUrl), storyInstance.MediaKey, storyInstance.ThumbnailIv,
		//		false);
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="storyInstance"></param>
		///// <returns></returns>
		//public byte[] GetStoryMediaBlob(Story storyInstance)
		//{
		//	return GetStoryBlob(new Uri(storyInstance.MediaUrl), storyInstance.MediaKey, storyInstance.MediaIv,
		//		storyInstance.Zipped);
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="blobPath"></param>
		///// <param name="key"></param>
		///// <param name="iv"></param>
		///// <param name="zipped"></param>
		///// <returns></returns>
		//public byte[] GetStoryBlob(Uri blobPath, string key, string iv, bool zipped)
		//{
		//	var data = _webConnect.GetBytes(blobPath);
		//	if (data == null) return null;
		//	var decompressedData = zipped ? Gzip.Decompress(data) : data;

		//	var decryptedData = Aes.DecryptDataWithIv(decompressedData, Convert.FromBase64String(key),
		//		Convert.FromBase64String(iv));

		//	return decryptedData;
		//}

		/// <summary>
		/// Downloads the blob data the snap contains
		/// </summary>
		/// <param name="snapId">The Id of the snap to download</param>
		/// <returns></returns>
		public async Task<byte[]> GetSnapBlobAsync(string snapId)
		{
			var timestamp = Timestamps.GenerateRetardedTimestamp();
			var postData = new Dictionary<string, string>
			{
				{"username", GetAuthedUsername()},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)},
				{"id", snapId}
			};

			var data =
				await
					_webConnect.PostToByteArrayAsync(SnapBlobEndpointUrl, postData, _snapchatManager.AuthToken,
						timestamp.ToString(CultureInfo.InvariantCulture));

			return Blob.ValidateMediaBlob(data)
				? data
				: Aes.DecryptData(data, Convert.FromBase64String(Settings.BlobEncryptionKey));
		}

		/// <summary>
		/// Downloads the blob data the snap contains
		/// </summary>
		/// <param name="snapId">The Id of the snap to download</param>
		/// <returns></returns>
		public byte[] GetSnapBlob(string snapId)
		{
			return AsyncHelpers.RunSync(() => GetSnapBlobAsync(snapId));
		}

		#endregion

		#region Chat Media

		/// <summary>
		/// 
		/// </summary>
		/// <param name="chatMediaId">The Id of the chat media.</param>
		/// <param name="iv"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public async Task<byte[]> GetChatMediaAsync(string chatMediaId, string iv, string key)
		{
			var timestamp = Timestamps.GenerateRetardedTimestamp();
			var postData = new Dictionary<string, string>
			{
				{"id", chatMediaId},
				{"username", GetAuthedUsername()},
				{"timestamp", timestamp.ToString(CultureInfo.InvariantCulture)}
			};

			var response = Aes.DecryptDataWithIv(
				(await
					_webConnect.PostToByteArrayAsync(ChatMediaEndpointUrl, postData, _snapchatManager.AuthToken,
						timestamp.ToString(CultureInfo.InvariantCulture))), Convert.FromBase64String(key), Convert.FromBase64String(iv));

			return response;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="chatMediaId">The Id of the chat media.</param>
		/// <param name="iv"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public byte[] GetChatMedia(string chatMediaId, string iv, string key)
		{
			return AsyncHelpers.RunSync(() => GetChatMediaAsync(chatMediaId, iv, key));
		}

		#endregion

		#endregion

		/// <summary>
		/// 
		/// </summary>
		private string GetAuthedUsername()
		{
			if (_snapchatManager.AllUpdates != null && _snapchatManager.AllUpdates.UpdatesResponse != null)
				return _snapchatManager.AllUpdates.UpdatesResponse.Username;

			throw new InvalidCredentialsException("There is no username set in the Snapchat Manager.");
		}

		/// <summary>
		/// 
		/// </summary>
		private string GetAuthToken()
		{
			return _snapchatManager.AuthToken;
		}
	}
}
