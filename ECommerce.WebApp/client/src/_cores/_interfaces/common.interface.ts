export interface ICity {
  code: string; // 1
  name: string; // "Thành phố Hà Nội"
  codeName: string; // "thanh_pho_ha_noi"
}

export interface IDistrict {
  code: string; // 1
  name: string; // "Thành phố Hà Nội"
  codeName: string; // "thanh_pho_ha_noi"
  provinceCode: string;
}

export interface IWard {
    code: string; // 1
    name: string; // "Thành phố Hà Nội"
    codeName: string; // "thanh_pho_ha_noi"
    districtCode: string;
}
