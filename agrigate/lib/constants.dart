import 'dart:io';

final kIsMobile = Platform.isAndroid || Platform.isIOS;

// API Request Header
const kApiHeaderName = 'AgrigateApiKey';

// Settings Keys
const kServerUrl = 'AgrigateServerUrl';
const kApiKey = 'AgrigateApiKey';
const kNotificationConnection = 'AgrigateNotificationConnection';
const kNotificationSecure = 'AgrigateNotificationSecure';
const kNotificationHost = 'AgrigateNotificationHost';
const kNotificationPort = 'AgrigateNotificationPort';
const kNotificationClient = 'AgrigateNotificationClient';
const kNotificationUser = 'AgrigateNotificationUser';
const kNotificationPassword = 'AgrigateNotificationPassword';
const kNotificationTopics = 'AgrigateNotificationTopics';

// Strings
const kAgrigate = 'Agrigate';

// Assets
const kLogoPath = 'assets/images/logo.png';

// Sizes
enum Size { small, medium, large }

const ksmall = 6.0;
const kMedium = 12.0;
const kLarge = 20.0;

const kLogoHeight = 60.0;
const kSmallBottomSheetHeight = 300.0;
