import { FOOTER_MENU_COL_1, FOOTER_MENU_COL_2, FOOTER_MENU_COL_3, FOOTER_MENU_COL_4 } from "src/_configs/web.config";
import MuiIcon from "src/_shares/_components/mui-icon/mui-icon.component";

const Footer = () => {
    return (
        <footer className="footer">
            <div className="custom-container">
                <div className="footer__inner">
                    <div className="py-4">
                        <div className="flex flex-wrap justify-between">
                            <div className="footer__info">
                                <span className="footer__info-title">
                                    <p>Công ty</p>
                                </span>
                                <ul className="footer__info-list">
                                    {FOOTER_MENU_COL_1.map((item: any) => (
                                        <li key={`footer-col-1-${item.path}`} className="footer__info-link pb-2">
                                            <a href={item.path}>{item.name}</a>
                                        </li>
                                    ))}
                                </ul>
                            </div>
                            <div className="footer__info">
                                <span className="footer__info-title">
                                    <p>Công ty</p>
                                </span>
                                <ul className="footer__info-list">
                                    {FOOTER_MENU_COL_2.map((item: any) => (
                                        <li key={`footer-col-1-${item.path}`} className="footer__info-link pb-2">
                                            <a href={item.path}>{item.name}</a>
                                        </li>
                                    ))}
                                </ul>
                            </div>
                            <div className="footer__info">
                                <span className="footer__info-title">
                                    <p>Tìm chúng tôi trên</p>
                                </span>
                                <ul className="footer__info-list">
                                    {FOOTER_MENU_COL_3.map((item: any) => (
                                        <li key={`footer-col-3-${item.path}`} className="footer__info-link pb-2">
                                            <a href={item.path} className="flex items-center" target="_blank" rel="noopener noreferrer">
                                                <MuiIcon name={item.icon} className='mr-3' fontSize="small" /> {item.name}
                                            </a>
                                        </li>
                                    ))}
                                </ul>
                            </div>
                            <div className="footer__info">
                                <span className="footer__info-title">
                                    <p>Công ty</p>
                                </span>
                                <ul className="footer__info-list">
                                    {FOOTER_MENU_COL_4.map((item: any) => (
                                        <li key={`footer-col-4-${item.path}`} className="footer__info-link pb-2">
                                            <a href={item.path} target="_blank" rel="noopener noreferrer">
                                                <MuiIcon name={item.icon} className='mr-2' fontSize="small" /> {item.name}
                                            </a>
                                        </li>
                                    ))}
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div className="footer__copyright">
                <div className="custom-container">
                    <div className="py-4">
                        <p className="footer__copyright-info">
                            <i className="far fa-copyright"></i> 2023 - Copyright by HiHiChi
                        </p>
                    </div>
                </div>
            </div>
        </footer>
    );
}

export default Footer;