import { ICON_NAME } from "./_enums/mui-icon.enum";

import FacebookIcon from "@mui/icons-material/Facebook";
import FacebookRoundedIcon from "@mui/icons-material/FacebookRounded";
import YouTubeIcon from "@mui/icons-material/YouTube";
import InstagramIcon from "@mui/icons-material/Instagram";
import SendRoundedIcon from "@mui/icons-material/SendRounded";
import PhoneAndroidIcon from "@mui/icons-material/PhoneAndroid";
import MailOutlinedIcon from "@mui/icons-material/MailOutlined";
import LocationOnOutlinedIcon from "@mui/icons-material/LocationOnOutlined";
import AccessTimeOutlinedIcon from "@mui/icons-material/AccessTimeOutlined";
import KeyboardArrowLeftRoundedIcon from "@mui/icons-material/KeyboardArrowLeftRounded";
import KeyboardArrowRightRoundedIcon from "@mui/icons-material/KeyboardArrowRightRounded";
import LockIcon from "@mui/icons-material/Lock";
import LocalMallOutlinedIcon from '@mui/icons-material/LocalMallOutlined';
import ArrowDropDownIcon from "@mui/icons-material/ArrowDropDown";
import StarRoundedIcon from "@mui/icons-material/StarRounded";
import PhoneOutlinedIcon from "@mui/icons-material/PhoneOutlined";
import EditOutlinedIcon from "@mui/icons-material/EditOutlined";
import ThumbUpOffAltIcon from "@mui/icons-material/ThumbUpOffAlt";
import ThumbUpAltIcon from "@mui/icons-material/ThumbUpAlt";
import ThumbDownAltIcon from "@mui/icons-material/ThumbDownAlt";
import ThumbDownOffAltIcon from "@mui/icons-material/ThumbDownOffAlt";
import MoreHorizIcon from "@mui/icons-material/MoreHoriz";
import VerifiedOutlinedIcon from '@mui/icons-material/VerifiedOutlined';
import FactCheckOutlinedIcon from '@mui/icons-material/FactCheckOutlined';
import ChatOutlinedIcon from '@mui/icons-material/ChatOutlined';
import LocalShippingOutlinedIcon from '@mui/icons-material/LocalShippingOutlined';

import { ReactComponent as MessengerIcon } from "src/_shares/_assets/_icons/messenger.svg";
import { ReactComponent as ChevronRightIcon } from "src/_shares/_assets/_icons/chevron-right.svg";
import { ReactComponent as ChevronLeftIcon } from "src/_shares/_assets/_icons/chevron-left.svg";
import { ReactComponent as ChevronDownIcon } from "src/_shares/_assets/_icons/chevron-down.svg";
import { ReactComponent as HelpCircleIcon } from "src/_shares/_assets/_icons/help-circle.svg";
import { ReactComponent as SmartPhoneIcon } from "src/_shares/_assets/_icons/smartphone.svg";
import { ReactComponent as UserIcon } from "src/_shares/_assets/_icons/user.svg";
import { ReactComponent as SearchIcon } from "src/_shares/_assets/_icons/search.svg";
import { ReactComponent as HeartIcon } from "src/_shares/_assets/_icons/heart.svg";
import { ReactComponent as ShoppingBagIcon } from "src/_shares/_assets/_icons/shopping-bag.svg";
import { ReactComponent as MenuIcon } from "src/_shares/_assets/_icons/menu.svg";
import { ReactComponent as XIcon } from "src/_shares/_assets/_icons/x.svg";
import { ReactComponent as ThumbsUpIcon } from "src/_shares/_assets/_icons/thumbs-up.svg";
import { ReactComponent as ThumbsDownIcon } from "src/_shares/_assets/_icons/thumbs-down.svg";
import { ReactComponent as PhoneIcon } from "src/_shares/_assets/_icons/phone.svg";
import { ReactComponent as StarIcon } from "src/_shares/_assets/_icons/star.svg";

const MuiIcon = (props: any) => {
  const { name } = props;
  switch (name) {
    // MUI DEFAULT ICONS
    case ICON_NAME.FACEBOOK:
      return <FacebookIcon {...props} />;
    case ICON_NAME.MESSENGER:
      return <MessengerIcon {...props} />;
    case ICON_NAME.ROUNDED.FACEBOOK:
      return <FacebookRoundedIcon {...props} />;
    case ICON_NAME.YOUTUBE:
      return <YouTubeIcon {...props} />;
    case ICON_NAME.INSTAGRAM:
      return <InstagramIcon {...props} />;
    case ICON_NAME.PHONE_ANDROID:
      return <PhoneAndroidIcon {...props} />;
    case ICON_NAME.PASSWORD:
      return <LockIcon {...props} />;
    case ICON_NAME.ARROW_DROP_DOWN:
      return <ArrowDropDownIcon {...props} />;
    case ICON_NAME.MORE_HORIZ:
      return <MoreHorizIcon {...props} />;
    case ICON_NAME.LIKE_FILL:
      return <ThumbUpAltIcon {...props} />;
    case ICON_NAME.DISLIKE_FILL:
      return <ThumbDownAltIcon {...props} />;

    // MUI OUTLINED ICONS
    case ICON_NAME.OUTLINED.MAIL:
      return <MailOutlinedIcon {...props} />;
    case ICON_NAME.OUTLINED.LOCATION:
      return <LocationOnOutlinedIcon {...props} />;
    case ICON_NAME.OUTLINED.ACCESS_TIME:
      return <AccessTimeOutlinedIcon {...props} />;
    case ICON_NAME.OUTLINED.LOCAL_MALL:
      return <LocalMallOutlinedIcon {...props} />
    case ICON_NAME.OUTLINED.VERIFIED:
      return <VerifiedOutlinedIcon {...props} />
    case ICON_NAME.OUTLINED.FACT_CHECK:
      return <FactCheckOutlinedIcon {...props} />
    case ICON_NAME.OUTLINED.CHAT:
      return <ChatOutlinedIcon {...props} />
    case ICON_NAME.OUTLINED.LOCAL_SHIPPING:
      return <LocalShippingOutlinedIcon {...props} />

    // MUI ROUNDED ICONS
    case ICON_NAME.ROUNDED.SEND:
      return <SendRoundedIcon {...props} />;
    case ICON_NAME.ROUNDED.ARROW_LEFT:
      return <KeyboardArrowLeftRoundedIcon {...props} />;
    case ICON_NAME.ROUNDED.ARROW_RIGHT:
      return <KeyboardArrowRightRoundedIcon {...props} />;
    case ICON_NAME.ROUNDED.STAR:
      return <StarRoundedIcon {...props} />;

    // FEATHER ICONS
    case ICON_NAME.FEATHER.CHEVRON_RIGHT:
      return <ChevronRightIcon {...props} />;
    case ICON_NAME.FEATHER.CHEVRON_LEFT:
      return <ChevronLeftIcon {...props} />;
    case ICON_NAME.FEATHER.CHEVRON_DOWN:
      return <ChevronDownIcon {...props} />;
    case ICON_NAME.FEATHER.HELP_CIRCLE:
      return <HelpCircleIcon {...props} />;
    case ICON_NAME.FEATHER.SMARTPHONE:
      return <SmartPhoneIcon {...props} />;
    case ICON_NAME.FEATHER.USER:
      return <UserIcon {...props} />;
    case ICON_NAME.FEATHER.SEARCH:
      return <SearchIcon {...props} />;
    case ICON_NAME.FEATHER.HEART:
      return <HeartIcon {...props} />;
    case ICON_NAME.FEATHER.SHOPPING_BAG:
      return <ShoppingBagIcon {...props} />;
    case ICON_NAME.FEATHER.LIKE:
      return <ThumbUpOffAltIcon {...props} />;
    case ICON_NAME.FEATHER.DISLIKE:
      return <ThumbDownOffAltIcon {...props} />;
    case ICON_NAME.FEATHER.PHONE:
      return <PhoneOutlinedIcon {...props} />;
    case ICON_NAME.FEATHER.EDIT:
      return <EditOutlinedIcon {...props} />;
    case ICON_NAME.FEATHER.MENU:
      return <MenuIcon {...props} />;
    case ICON_NAME.FEATHER.X:
      return <XIcon {...props} />;
    case ICON_NAME.FEATHER.THUMBS_UP:
      return <ThumbsUpIcon {...props} />;
    case ICON_NAME.FEATHER.THUMBS_DOWN:
      return <ThumbsDownIcon {...props} />;
    case ICON_NAME.FEATHER.PHONE_PRODUCT_DETAIL:
      return <PhoneIcon {...props} />;
    case ICON_NAME.FEATHER.STAR:
      return <StarIcon {...props} />;

    default:
      return <></>;
  }
};

export default MuiIcon;
