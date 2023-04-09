import { ICON_NAME } from "./_enums/mui-icon.enum";
import FacebookIcon from '@mui/icons-material/Facebook';
import FacebookRoundedIcon from '@mui/icons-material/FacebookRounded';
import YouTubeIcon from '@mui/icons-material/YouTube';
import InstagramIcon from '@mui/icons-material/Instagram';
import SendRoundedIcon from '@mui/icons-material/SendRounded';
import PhoneAndroidIcon from '@mui/icons-material/PhoneAndroid';
import MailOutlinedIcon from '@mui/icons-material/MailOutlined';
import LocationOnOutlinedIcon from '@mui/icons-material/LocationOnOutlined';
import AccessTimeOutlinedIcon from '@mui/icons-material/AccessTimeOutlined';

const MuiIcon = (props: any) => {
    const { name } = props;
    switch (name) {
        case ICON_NAME.FACEBOOK: return <FacebookIcon {...props} />
        case ICON_NAME.FACEBOOK_ROUNDED: return <FacebookRoundedIcon {...props} />
        case ICON_NAME.YOUTUBE: return <YouTubeIcon {...props} />
        case ICON_NAME.INSTAGRAM: return <InstagramIcon {...props} />
        case ICON_NAME.SEND_ROUNDED: return <SendRoundedIcon {...props} />
        case ICON_NAME.PHONE_ANDROID: return <PhoneAndroidIcon {...props} />
        case ICON_NAME.MAIL_OUTLINED: return <MailOutlinedIcon {...props} />
        case ICON_NAME.LOCATION_OUTLINED: return <LocationOnOutlinedIcon {...props} />
        case ICON_NAME.ACCESS_TIME_OUTLINED: return <AccessTimeOutlinedIcon {...props} />
        default: return <></>
    }
}

export default MuiIcon;