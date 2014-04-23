// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

using System;
using System.Text;
#if !WINRT_NOT_PRESENT
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
#endif

namespace SnapDotNet.Apps
{
	public static partial class Notifications
	{
		public static class ToastContent
		{
			private sealed class ToastAudio : IToastAudio
			{
				public ToastAudio() { }

				public ToastAudioContent Content
				{
					get { return m_Content; }
					set
					{
						if (!Enum.IsDefined(typeof(ToastAudioContent), value))
						{
							throw new ArgumentOutOfRangeException("value");
						}
						m_Content = value;
					}
				}

				public bool Loop
				{
					get { return m_Loop; }
					set { m_Loop = value; }
				}

				private ToastAudioContent m_Content = ToastAudioContent.Default;
				private bool m_Loop = false;
			}

			private sealed class IncomingCallCommands : IIncomingCallCommands
			{
				public IncomingCallCommands() { }

				public bool ShowVideoCommand
				{
					get { return m_Video; }
					set { m_Video = value; }
				}

				public string VideoArgument
				{
					get { return m_VideoArgument; }
					set { m_VideoArgument = value; }
				}

				public bool ShowVoiceCommand
				{
					get { return m_Voice; }
					set { m_Voice = value; }
				}

				public string VoiceArgument
				{
					get { return m_VoiceArgument; }
					set { m_VoiceArgument = value; }
				}

				public bool ShowDeclineCommand
				{
					get { return m_Decline; }
					set { m_Decline = value; }
				}

				public string DeclineArgument
				{
					get { return m_DeclineArgument; }
					set { m_DeclineArgument = value; }
				}

				private bool m_Video = false;
				private bool m_Voice = false;
				private bool m_Decline = false;

				private string m_VideoArgument = string.Empty;
				private string m_VoiceArgument = string.Empty;
				private string m_DeclineArgument = string.Empty;
			}

			internal sealed class AlarmCommands : IAlarmCommands
			{
				internal AlarmCommands() { }

				public bool ShowSnoozeCommand
				{
					get { return m_Snooze; }
					set { m_Snooze = value; }
				}

				public string SnoozeArgument
				{
					get { return m_SnoozeArgument; }
					set { m_SnoozeArgument = value; }
				}

				public bool ShowDismissCommand
				{
					get { return m_Dismiss; }
					set { m_Dismiss = value; }
				}

				public string DismissArgument
				{
					get { return m_DismissArgument; }
					set { m_DismissArgument = value; }
				}

				private bool m_Snooze = false;
				private bool m_Dismiss = false;

				private string m_SnoozeArgument = string.Empty;
				private string m_DismissArgument = string.Empty;
			}


			private class ToastNotificationBase : NotificationBase, IToastNotificationContent
			{
				public ToastNotificationBase(string templateName, int imageCount, int textCount) :
					base(templateName: templateName, fallbackName: null, imageCount: imageCount, textCount: textCount)
				{
				}

				private bool AudioSrcIsLooping()
				{
					return
						(Audio.Content == ToastAudioContent.LoopingAlarm) ||
						(Audio.Content == ToastAudioContent.LoopingAlarm2) ||
						(Audio.Content == ToastAudioContent.LoopingAlarm3) ||
						(Audio.Content == ToastAudioContent.LoopingAlarm4) ||
						(Audio.Content == ToastAudioContent.LoopingAlarm5) ||
						(Audio.Content == ToastAudioContent.LoopingAlarm6) ||
						(Audio.Content == ToastAudioContent.LoopingAlarm7) ||
						(Audio.Content == ToastAudioContent.LoopingAlarm8) ||
						(Audio.Content == ToastAudioContent.LoopingAlarm9) ||
						(Audio.Content == ToastAudioContent.LoopingAlarm10) ||
						(Audio.Content == ToastAudioContent.LoopingCall) ||
						(Audio.Content == ToastAudioContent.LoopingCall2) ||
						(Audio.Content == ToastAudioContent.LoopingCall3) ||
						(Audio.Content == ToastAudioContent.LoopingCall4) ||
						(Audio.Content == ToastAudioContent.LoopingCall5) ||
						(Audio.Content == ToastAudioContent.LoopingCall6) ||
						(Audio.Content == ToastAudioContent.LoopingCall7) ||
						(Audio.Content == ToastAudioContent.LoopingCall8) ||
						(Audio.Content == ToastAudioContent.LoopingCall9) ||
						(Audio.Content == ToastAudioContent.LoopingCall10);
				}

				private void ValidateAudio()
				{
					if (StrictValidation)
					{
						if (Audio.Loop && Duration != ToastDuration.Long)
						{
							throw new NotificationContentValidationException("Looping audio is only available for long duration toasts.");
						}
						if (Audio.Loop && !AudioSrcIsLooping())
						{
							throw new NotificationContentValidationException(
								"A looping audio src must be chosen if the looping audio property is set.");
						}
						if (!Audio.Loop && AudioSrcIsLooping())
						{
							throw new NotificationContentValidationException(
								"The looping audio property needs to be set if a looping audio src is chosen.");
						}
					}
				}

				private void AppendAudioTag(StringBuilder builder)
				{
					if (Audio.Content != ToastAudioContent.Default)
					{
						builder.Append("<audio");
						if (Audio.Content == ToastAudioContent.Silent)
						{
							builder.Append(" silent='true'/>");
						}
						else
						{
							if (Audio.Loop == true)
							{
								builder.Append(" loop='true'");
							}

							// The default looping sound is LoopingCall - save size by not adding it.
							if (Audio.Content != ToastAudioContent.LoopingCall)
							{
								string audioSrc = null;
								switch (Audio.Content)
								{
									case ToastAudioContent.IM:
										audioSrc = "ms-winsoundevent:Notification.IM";
										break;
									case ToastAudioContent.Mail:
										audioSrc = "ms-winsoundevent:Notification.Mail";
										break;
									case ToastAudioContent.Reminder:
										audioSrc = "ms-winsoundevent:Notification.Reminder";
										break;
									case ToastAudioContent.SMS:
										audioSrc = "ms-winsoundevent:Notification.SMS";
										break;
									case ToastAudioContent.LoopingAlarm:
										audioSrc = "ms-winsoundevent:Notification.Looping.Alarm";
										break;
									case ToastAudioContent.LoopingAlarm2:
										audioSrc = "ms-winsoundevent:Notification.Looping.Alarm2";
										break;
									case ToastAudioContent.LoopingAlarm3:
										audioSrc = "ms-winsoundevent:Notification.Looping.Alarm3";
										break;
									case ToastAudioContent.LoopingAlarm4:
										audioSrc = "ms-winsoundevent:Notification.Looping.Alarm4";
										break;
									case ToastAudioContent.LoopingAlarm5:
										audioSrc = "ms-winsoundevent:Notification.Looping.Alarm5";
										break;
									case ToastAudioContent.LoopingAlarm6:
										audioSrc = "ms-winsoundevent:Notification.Looping.Alarm6";
										break;
									case ToastAudioContent.LoopingAlarm7:
										audioSrc = "ms-winsoundevent:Notification.Looping.Alarm7";
										break;
									case ToastAudioContent.LoopingAlarm8:
										audioSrc = "ms-winsoundevent:Notification.Looping.Alarm8";
										break;
									case ToastAudioContent.LoopingAlarm9:
										audioSrc = "ms-winsoundevent:Notification.Looping.Alarm9";
										break;
									case ToastAudioContent.LoopingAlarm10:
										audioSrc = "ms-winsoundevent:Notification.Looping.Alarm10";
										break;
									case ToastAudioContent.LoopingCall:
										audioSrc = "ms-winsoundevent:Notification.Looping.Call";
										break;
									case ToastAudioContent.LoopingCall2:
										audioSrc = "ms-winsoundevent:Notification.Looping.Call2";
										break;
									case ToastAudioContent.LoopingCall3:
										audioSrc = "ms-winsoundevent:Notification.Looping.Call3";
										break;
									case ToastAudioContent.LoopingCall4:
										audioSrc = "ms-winsoundevent:Notification.Looping.Call4";
										break;
									case ToastAudioContent.LoopingCall5:
										audioSrc = "ms-winsoundevent:Notification.Looping.Call5";
										break;
									case ToastAudioContent.LoopingCall6:
										audioSrc = "ms-winsoundevent:Notification.Looping.Call6";
										break;
									case ToastAudioContent.LoopingCall7:
										audioSrc = "ms-winsoundevent:Notification.Looping.Call7";
										break;
									case ToastAudioContent.LoopingCall8:
										audioSrc = "ms-winsoundevent:Notification.Looping.Call8";
										break;
									case ToastAudioContent.LoopingCall9:
										audioSrc = "ms-winsoundevent:Notification.Looping.Call9";
										break;
									case ToastAudioContent.LoopingCall10:
										audioSrc = "ms-winsoundevent:Notification.Looping.Call10";
										break;
								}
								builder.AppendFormat(" src='{0}'", audioSrc);
							}
						}
						builder.Append("/>");
					}
				}

				private bool IsIncomingCallToast()
				{
					return (IncomingCallCommands.ShowVideoCommand || IncomingCallCommands.ShowVoiceCommand || IncomingCallCommands.ShowDeclineCommand);
				}

				private bool IsAlarmToast()
				{
					return (AlarmCommands.ShowSnoozeCommand || AlarmCommands.ShowDismissCommand);
				}

				private void ValidateCommands()
				{
					if (StrictValidation)
					{
						if (IsIncomingCallToast() && IsAlarmToast())
						{
							throw new NotificationContentValidationException(
									"IncomingCall and Alarm properties cannot be included on a single toast.");
						}

						if (!IncomingCallCommands.ShowVideoCommand && !string.IsNullOrEmpty(IncomingCallCommands.VideoArgument))
						{
							throw new NotificationContentValidationException(
									"Video argument should not be set if the Video button is not shown.");
						}

						if (!IncomingCallCommands.ShowVoiceCommand && !string.IsNullOrEmpty(IncomingCallCommands.VoiceArgument))
						{
							throw new NotificationContentValidationException(
									"Voice argument should not be set if the Voice button is not shown.");
						}

						if (!IncomingCallCommands.ShowDeclineCommand && !string.IsNullOrEmpty(IncomingCallCommands.DeclineArgument))
						{
							throw new NotificationContentValidationException(
									"Decline argument should not be set if the Video button is not shown.");
						}

						if (!AlarmCommands.ShowSnoozeCommand && !string.IsNullOrEmpty(AlarmCommands.SnoozeArgument))
						{
							throw new NotificationContentValidationException(
									"Snooze argument should not be set if the Snooze button is not shown.");
						}

						if (!AlarmCommands.ShowDismissCommand && !string.IsNullOrEmpty(AlarmCommands.DismissArgument))
						{
							throw new NotificationContentValidationException(
									"Dismiss argument should not be set if the Dismiss button is not shown.");
						}
					}
				}

				private void AppendIncomingCallCommandsTag(StringBuilder builder)
				{
					builder.Append("<commands scenario='incomingcall'>");
					string CommandIDFormat = "<command id='{0}'";
					string ArgumentFormat = "arguments='{0}'";
					if (IncomingCallCommands.ShowVideoCommand)
					{
						builder.AppendFormat(CommandIDFormat, "video");
						if (!string.IsNullOrEmpty(IncomingCallCommands.VideoArgument))
						{
							builder.AppendFormat(ArgumentFormat, Util.HttpEncode(IncomingCallCommands.VideoArgument));
						}
						builder.Append("/>");
					}
					if (IncomingCallCommands.ShowVoiceCommand)
					{
						builder.AppendFormat(CommandIDFormat, "voice");
						if (!string.IsNullOrEmpty(IncomingCallCommands.VoiceArgument))
						{
							builder.AppendFormat(ArgumentFormat, Util.HttpEncode(IncomingCallCommands.VoiceArgument));
						}
						builder.Append("/>");
					}
					if (IncomingCallCommands.ShowDeclineCommand)
					{
						builder.AppendFormat(CommandIDFormat, "decline");
						if (!string.IsNullOrEmpty(IncomingCallCommands.DeclineArgument))
						{
							builder.AppendFormat(ArgumentFormat, Util.HttpEncode(IncomingCallCommands.DeclineArgument));
						}
						builder.Append("/>");
					}
					builder.Append("</commands>");
				}

				private void AppendAlarmCommandsTag(StringBuilder builder)
				{
					builder.Append("<commands scenario='alarm'>");
					string CommandIDFormat = "<command id='{0}'";
					string ArgumentFormat = "arguments='{0}'";
					if (AlarmCommands.ShowSnoozeCommand)
					{
						builder.AppendFormat(CommandIDFormat, "snooze");
						if (!string.IsNullOrEmpty(AlarmCommands.SnoozeArgument))
						{
							builder.AppendFormat(ArgumentFormat, Util.HttpEncode(AlarmCommands.SnoozeArgument));
						}
						builder.Append("/>");
					}
					if (AlarmCommands.ShowDismissCommand)
					{
						builder.AppendFormat(CommandIDFormat, "dismiss");
						if (!string.IsNullOrEmpty(AlarmCommands.DismissArgument))
						{
							builder.AppendFormat(ArgumentFormat, Util.HttpEncode(AlarmCommands.DismissArgument));
						}
						builder.Append("/>");
					}
					builder.Append("</commands>");
				}

				public override string GetContent()
				{
					ValidateAudio();
					ValidateCommands();

					StringBuilder builder = new StringBuilder("<toast");
					if (!String.IsNullOrEmpty(Launch))
					{
						builder.AppendFormat(" launch='{0}'", Util.HttpEncode(Launch));
					}
					if (Duration != ToastDuration.Short)
					{
						builder.AppendFormat(" duration='{0}'", Duration.ToString().ToLowerInvariant());
					}
					builder.Append(">");

					builder.AppendFormat("<visual version='{0}'", Util.NOTIFICATION_CONTENT_VERSION);
					if (!String.IsNullOrWhiteSpace(Lang))
					{
						builder.AppendFormat(" lang='{0}'", Util.HttpEncode(Lang));
					}
					if (!String.IsNullOrWhiteSpace(BaseUri))
					{
						builder.AppendFormat(" baseUri='{0}'", Util.HttpEncode(BaseUri));
					}
					if (AddImageQuery)
					{
						builder.AppendFormat(" addImageQuery='true'");
					}
					builder.Append(">");

					builder.AppendFormat("<binding template='{0}'>{1}</binding>", TemplateName, SerializeProperties(Lang, BaseUri, AddImageQuery));
					builder.Append("</visual>");

					AppendAudioTag(builder);

					if (IsIncomingCallToast())
					{
						AppendIncomingCallCommandsTag(builder);
					}

					if (IsAlarmToast())
					{
						AppendAlarmCommandsTag(builder);
					}

					builder.Append("</toast>");

					return builder.ToString();
				}

#if !WINRT_NOT_PRESENT
				public ToastNotification CreateNotification()
				{
					if (StrictValidation)
					{
						if (IsAlarmToast())
						{
							throw new NotificationContentValidationException(
								"The ToastNotification object should not be used for alarms." +
								"Use the results of GetContent() to construct a ScheduledToastNotification object.");
						}
					}
					XmlDocument xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(GetContent());
					return new ToastNotification(xmlDoc);
				}
#endif

				public IToastAudio Audio
				{
					get { return m_Audio; }
				}

				public IIncomingCallCommands IncomingCallCommands
				{
					get { return m_IncomingCallCommands; }
				}

				public IAlarmCommands AlarmCommands
				{
					get { return m_AlarmCommands; }
				}

				public string Launch
				{
					get { return m_Launch; }
					set { m_Launch = value; }
				}

				public ToastDuration Duration
				{
					get { return m_Duration; }
					set
					{
						if (!Enum.IsDefined(typeof(ToastDuration), value))
						{
							throw new ArgumentOutOfRangeException("value");
						}
						m_Duration = value;
					}
				}

				private string m_Launch;
				private IToastAudio m_Audio = new ToastAudio();
				private IIncomingCallCommands m_IncomingCallCommands = new IncomingCallCommands();
				private IAlarmCommands m_AlarmCommands = new AlarmCommands();
				private ToastDuration m_Duration = ToastDuration.Short;
			}

			private class ToastImageAndText01 : ToastNotificationBase, IToastImageAndText01
			{
				public ToastImageAndText01()
					: base(templateName: "ToastImageAndText01", imageCount: 1, textCount: 1)
				{
				}

				public INotificationContentImage Image { get { return Images[0]; } }

				public INotificationContentText TextBodyWrap { get { return TextFields[0]; } }
			}

			private class ToastImageAndText02 : ToastNotificationBase, IToastImageAndText02
			{
				public ToastImageAndText02()
					: base(templateName: "ToastImageAndText02", imageCount: 1, textCount: 2)
				{
				}

				public INotificationContentImage Image { get { return Images[0]; } }

				public INotificationContentText TextHeading { get { return TextFields[0]; } }
				public INotificationContentText TextBodyWrap { get { return TextFields[1]; } }
			}

			private class ToastImageAndText03 : ToastNotificationBase, IToastImageAndText03
			{
				public ToastImageAndText03()
					: base(templateName: "ToastImageAndText03", imageCount: 1, textCount: 2)
				{
				}

				public INotificationContentImage Image { get { return Images[0]; } }

				public INotificationContentText TextHeadingWrap { get { return TextFields[0]; } }
				public INotificationContentText TextBody { get { return TextFields[1]; } }
			}

			private class ToastImageAndText04 : ToastNotificationBase, IToastImageAndText04
			{
				public ToastImageAndText04()
					: base(templateName: "ToastImageAndText04", imageCount: 1, textCount: 3)
				{
				}

				public INotificationContentImage Image { get { return Images[0]; } }

				public INotificationContentText TextHeading { get { return TextFields[0]; } }
				public INotificationContentText TextBody1 { get { return TextFields[1]; } }
				public INotificationContentText TextBody2 { get { return TextFields[2]; } }
			}

			private class ToastText01 : ToastNotificationBase, IToastText01
			{
				public ToastText01()
					: base(templateName: "ToastText01", imageCount: 0, textCount: 1)
				{
				}

				public INotificationContentText TextBodyWrap { get { return TextFields[0]; } }
			}

			private class ToastText02 : ToastNotificationBase, IToastText02
			{
				public ToastText02()
					: base(templateName: "ToastText02", imageCount: 0, textCount: 2)
				{
				}

				public INotificationContentText TextHeading { get { return TextFields[0]; } }
				public INotificationContentText TextBodyWrap { get { return TextFields[1]; } }
			}

			private class ToastText03 : ToastNotificationBase, IToastText03
			{
				public ToastText03()
					: base(templateName: "ToastText03", imageCount: 0, textCount: 2)
				{
				}

				public INotificationContentText TextHeadingWrap { get { return TextFields[0]; } }
				public INotificationContentText TextBody { get { return TextFields[1]; } }
			}

			private class ToastText04 : ToastNotificationBase, IToastText04
			{
				public ToastText04()
					: base(templateName: "ToastText04", imageCount: 0, textCount: 3)
				{
				}

				public INotificationContentText TextHeading { get { return TextFields[0]; } }
				public INotificationContentText TextBody1 { get { return TextFields[1]; } }
				public INotificationContentText TextBody2 { get { return TextFields[2]; } }
			}

			/// <summary>
			/// A factory which creates toast content objects for all of the toast template types.
			/// </summary>
			public static class ToastContentFactory
			{
				/// <summary>
				/// Creates a ToastImageAndText01 template content object.
				/// </summary>
				/// <returns>A ToastImageAndText01 template content object.</returns>
				public static IToastImageAndText01 CreateToastImageAndText01()
				{
					return new ToastImageAndText01();
				}

				/// <summary>
				/// Creates a ToastImageAndText02 template content object.
				/// </summary>
				/// <returns>A ToastImageAndText02 template content object.</returns>
				public static IToastImageAndText02 CreateToastImageAndText02()
				{
					return new ToastImageAndText02();
				}

				/// <summary>
				/// Creates a ToastImageAndText03 template content object.
				/// </summary>
				/// <returns>A ToastImageAndText03 template content object.</returns>
				public static IToastImageAndText03 CreateToastImageAndText03()
				{
					return new ToastImageAndText03();
				}

				/// <summary>
				/// Creates a ToastImageAndText04 template content object.
				/// </summary>
				/// <returns>A ToastImageAndText04 template content object.</returns>
				public static IToastImageAndText04 CreateToastImageAndText04()
				{
					return new ToastImageAndText04();
				}

				/// <summary>
				/// Creates a ToastText01 template content object.
				/// </summary>
				/// <returns>A ToastText01 template content object.</returns>
				public static IToastText01 CreateToastText01()
				{
					return new ToastText01();
				}

				/// <summary>
				/// Creates a ToastText02 template content object.
				/// </summary>
				/// <returns>A ToastText02 template content object.</returns>
				public static IToastText02 CreateToastText02()
				{
					return new ToastText02();
				}

				/// <summary>
				/// Creates a ToastText03 template content object.
				/// </summary>
				/// <returns>A ToastText03 template content object.</returns>
				public static IToastText03 CreateToastText03()
				{
					return new ToastText03();
				}

				/// <summary>
				/// Creates a ToastText04 template content object.
				/// </summary>
				/// <returns>A ToastText04 template content object.</returns>
				public static IToastText04 CreateToastText04()
				{
					return new ToastText04();
				}
			}

			/// <summary>
			/// Type representing the toast notification audio properties which is contained within
			/// a toast notification content object.
			/// </summary>
			public interface IToastAudio
			{
				/// <summary>
				/// The audio content that should be played when the toast is shown.
				/// </summary>
				ToastAudioContent Content { get; set; }

				/// <summary>
				/// Whether the audio should loop.  If this property is set to true, the toast audio content
				/// must be a looping sound.
				/// </summary>
				bool Loop { get; set; }
			}

			/// <summary>
			/// Type representing the incoming call command properties which is contained within
			/// a toast notification content object.
			/// </summary>
			public interface IIncomingCallCommands
			{
				/// <summary>
				/// Whether the toast shows the video button.
				/// </summary>
				bool ShowVideoCommand { get; set; }

				/// <summary>
				/// The argument passed to the app when the video command is invoked.
				/// </summary>
				string VideoArgument { get; set; }

				/// <summary>
				/// Whether the toast shows the voice button.
				/// </summary>
				bool ShowVoiceCommand { get; set; }

				/// <summary>
				/// The argument passed to the app when the voice command is invoked.
				/// </summary>
				string VoiceArgument { get; set; }

				/// <summary>
				/// Whether the toast shows the decline button.
				/// </summary>
				bool ShowDeclineCommand { get; set; }

				/// <summary>
				/// The argument passed to the app when the decline command is invoked.
				/// </summary>
				string DeclineArgument { get; set; }
			}

			/// <summary>
			/// Type representing the alarm command properties which is contained within
			/// a toast notification content object.
			/// </summary>
			public interface IAlarmCommands
			{
				/// <summary>
				/// Whether the toast shows the snooze button.
				/// </summary>
				bool ShowSnoozeCommand { get; set; }

				/// <summary>
				/// The argument passed to the app when the snooze command is invoked.
				/// </summary>
				string SnoozeArgument { get; set; }

				/// <summary>
				/// Whether the toast shows the dismiss button.
				/// </summary>
				bool ShowDismissCommand { get; set; }

				/// <summary>
				/// The argument passed to the app when the dismiss command is invoked.
				/// </summary>
				string DismissArgument { get; set; }
			}

			/// <summary>
			/// Base toast notification content interface.
			/// </summary>
			public interface IToastNotificationContent : INotificationContent
			{
				/// <summary>
				/// Whether strict validation should be applied when the Xml or notification object is created,
				/// and when some of the properties are assigned.
				/// </summary>
				bool StrictValidation { get; set; }

				/// <summary>
				/// The language of the content being displayed.  The language should be specified using the
				/// abbreviated language code as defined by BCP 47.
				/// </summary>
				string Lang { get; set; }

				/// <summary>
				/// The BaseUri that should be used for image locations.  Relative image locations use this
				/// field as their base Uri.  The BaseUri must begin with http://, https://, ms-appx:///, or
				/// ms-appdata:///local/.
				/// </summary>
				string BaseUri { get; set; }

				/// <summary>
				/// Controls if query strings that denote the client configuration of contrast, scale, and language setting should be appended to the Src
				/// If true, Windows will append query strings onto images that exist in this template
				/// If false (the default, Windows will not append query strings onto images that exist in this template
				/// Query string details:
				/// Parameter: ms-contrast
				///     Values: standard, black, white
				/// Parameter: ms-scale
				///     Values: 80, 100, 140, 180
				/// Parameter: ms-lang
				///     Values: The BCP 47 language tag set in the notification xml, or if omitted, the current preferred language of the user
				/// </summary>
				bool AddImageQuery { get; set; }

				/// <summary>
				/// The launch parameter passed into the Windows Store app when the toast is activated.
				/// </summary>
				string Launch { get; set; }

				/// <summary>
				/// The audio that should be played when the toast is displayed.
				/// </summary>
				IToastAudio Audio { get; }

				/// <summary>
				/// The length that the toast should be displayed on screen.
				/// </summary>
				ToastDuration Duration { get; set; }

				/// <summary>
				/// IncomingCall action buttons will be displayed on the toast if one of the option flags (ShowVideoCommand/ShowVoiceCommand/ShowDeclineCommand) is set to true.
				/// To enable IncomingCall toasts for an app, ensure that the Lock Screen Call extension is enabled in the application manifest.
				/// </summary>
				IIncomingCallCommands IncomingCallCommands { get; }

				/// <summary>
				/// Alarm action buttons will be displayed on the toast if one of the option flags (ShowSnoozeCommand/ShowDismissCommand) is set to true.
				/// To enable Alarm toasts for an app, ensure the app declares itself as Alarm capable.
				/// The app needs to be set as the Primary Alarm on the PC Settings page (only one app can exist as the alarm app at a given time).
				/// </summary>
				IAlarmCommands AlarmCommands { get; }

#if !WINRT_NOT_PRESENT
				/// <summary>
				/// Creates a WinRT ToastNotification object based on the content.
				/// </summary>
				/// <returns>A WinRT ToastNotification object based on the content.</returns>
				ToastNotification CreateNotification();
#endif
			}

			/// <summary>
			/// The audio options that can be played while the toast is on screen.
			/// </summary>
			public enum ToastAudioContent
			{
				/// <summary>
				/// The default toast audio sound.
				/// </summary>
				Default = 0,
				/// <summary>
				/// Audio that corresponds to new mail arriving.
				/// </summary>
				Mail,
				/// <summary>
				/// Audio that corresponds to a new SMS message arriving.
				/// </summary>
				SMS,
				/// <summary>
				/// Audio that corresponds to a new IM arriving.
				/// </summary>
				IM,
				/// <summary>
				/// Audio that corresponds to a reminder.
				/// </summary>
				Reminder,
				/// <summary>
				/// The default looping sound.  Audio that corresponds to a call.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingCall,
				/// <summary>
				/// Audio that corresponds to a call.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingCall2,
				/// <summary>
				/// Audio that corresponds to a call.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingCall3,
				/// <summary>
				/// Audio that corresponds to a call.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingCall4,
				/// <summary>
				/// Audio that corresponds to a call.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingCall5,
				/// <summary>
				/// Audio that corresponds to a call.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingCall6,
				/// <summary>
				/// Audio that corresponds to a call.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingCall7,
				/// <summary>
				/// Audio that corresponds to a call.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingCall8,
				/// <summary>
				/// Audio that corresponds to a call.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingCall9,
				/// <summary>
				/// Audio that corresponds to a call.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingCall10,
				/// <summary>
				/// Audio that corresponds to an alarm.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingAlarm,
				/// <summary>
				/// Audio that corresponds to an alarm.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingAlarm2,
				/// <summary>
				/// Audio that corresponds to an alarm.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingAlarm3,
				/// <summary>
				/// Audio that corresponds to an alarm.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingAlarm4,
				/// <summary>
				/// Audio that corresponds to an alarm.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingAlarm5,
				/// <summary>
				/// Audio that corresponds to an alarm.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingAlarm6,
				/// <summary>
				/// Audio that corresponds to an alarm.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingAlarm7,
				/// <summary>
				/// Audio that corresponds to an alarm.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingAlarm8,
				/// <summary>
				/// Audio that corresponds to an alarm.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingAlarm9,
				/// <summary>
				/// Audio that corresponds to an alarm.
				/// Only valid for toasts that are have the duration set to "Long".
				/// </summary>
				LoopingAlarm10,
				/// <summary>
				/// No audio should be played when the toast is displayed.
				/// </summary>
				Silent
			}

			/// <summary>
			/// The duration the toast should be displayed on screen.
			/// </summary>
			public enum ToastDuration
			{
				/// <summary>
				/// Default behavior.  The toast will be on screen for a short amount of time.
				/// </summary>
				Short = 0,
				/// <summary>
				/// The toast will be on screen for a longer amount of time.
				/// </summary>
				Long
			}

			/// <summary>
			/// A toast template that displays an image and a text field.
			/// </summary>
			public interface IToastImageAndText01 : IToastNotificationContent
			{
				/// <summary>
				/// The main image on the toast.
				/// </summary>
				INotificationContentImage Image { get; }

				/// <summary>
				/// A body text field.
				/// </summary>
				INotificationContentText TextBodyWrap { get; }
			}

			/// <summary>
			/// A toast template that displays an image and two text fields.
			/// </summary>
			public interface IToastImageAndText02 : IToastNotificationContent
			{
				/// <summary>
				/// The main image on the toast.
				/// </summary>
				INotificationContentImage Image { get; }

				/// <summary>
				/// A heading text field.
				/// </summary>
				INotificationContentText TextHeading { get; }

				/// <summary>
				/// A body text field.
				/// </summary>
				INotificationContentText TextBodyWrap { get; }
			}

			/// <summary>
			/// A toast template that displays an image and two text fields.
			/// </summary>
			public interface IToastImageAndText03 : IToastNotificationContent
			{
				/// <summary>
				/// The main image on the toast.
				/// </summary>
				INotificationContentImage Image { get; }

				/// <summary>
				/// A heading text field.
				/// </summary>
				INotificationContentText TextHeadingWrap { get; }

				/// <summary>
				/// A body text field.
				/// </summary>
				INotificationContentText TextBody { get; }
			}

			/// <summary>
			/// A toast template that displays an image and three text fields.
			/// </summary>
			public interface IToastImageAndText04 : IToastNotificationContent
			{
				/// <summary>
				/// The main image on the toast.
				/// </summary>
				INotificationContentImage Image { get; }

				/// <summary>
				/// A heading text field.
				/// </summary>
				INotificationContentText TextHeading { get; }

				/// <summary>
				/// A body text field.
				/// </summary>
				INotificationContentText TextBody1 { get; }

				/// <summary>
				/// A body text field.
				/// </summary>
				INotificationContentText TextBody2 { get; }
			}

			/// <summary>
			/// A toast template that displays a text fields.
			/// </summary>
			public interface IToastText01 : IToastNotificationContent
			{
				/// <summary>
				/// A body text field.
				/// </summary>
				INotificationContentText TextBodyWrap { get; }
			}

			/// <summary>
			/// A toast template that displays two text fields.
			/// </summary>
			public interface IToastText02 : IToastNotificationContent
			{
				/// <summary>
				/// A heading text field.
				/// </summary>
				INotificationContentText TextHeading { get; }

				/// <summary>
				/// A body text field.
				/// </summary>
				INotificationContentText TextBodyWrap { get; }
			}

			/// <summary>
			/// A toast template that displays two text fields.
			/// </summary>
			public interface IToastText03 : IToastNotificationContent
			{
				/// <summary>
				/// A heading text field.
				/// </summary>
				INotificationContentText TextHeadingWrap { get; }

				/// <summary>
				/// A body text field.
				/// </summary>
				INotificationContentText TextBody { get; }
			}

			/// <summary>
			/// A toast template that displays three text fields.
			/// </summary>
			public interface IToastText04 : IToastNotificationContent
			{
				/// <summary>
				/// A heading text field.
				/// </summary>
				INotificationContentText TextHeading { get; }

				/// <summary>
				/// A body text field.
				/// </summary>
				INotificationContentText TextBody1 { get; }

				/// <summary>
				/// A body text field.
				/// </summary>
				INotificationContentText TextBody2 { get; }
			}
		}
	}
}