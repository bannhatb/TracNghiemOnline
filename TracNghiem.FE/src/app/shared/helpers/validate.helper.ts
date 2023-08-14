export default class ValidationHelper {
  private static validationMessages = {
    required: 'Trường này là bắt buộc nhập',
    // (This field is required)
    email: 'Email không đúng định dạng',
    // (Invalid email address)
    website: 'Website không đúng định dạng',
    // (Invalid URL format)
    probability:
      'Giá trị nằm trong khoảng từ 0-100',
    //   (The allowed value is between 0-100)
    phoneNumber:
      'Số điện thoại phải có ít nhất 10 chữ số',
    //   (Phone number must be at least 10 digits)
    faxNumber:
      'Fax phải có ít nhất 10 chữ số',
    //   (Fax number must be at least 10 digits)
    password:
      // tslint:disable-next-line:max-line-length
      'Mật khẩu phải có ít nhất 8 kí tự, trong đó có it nhất 1 chữ hoa, 1 chữ thường và 1 chữ số.',
    // tslint:disable-next-line:max-line-length
    //   (Password must contain at least 8 characters with at least one Capital letter, at least one lower case letter and at least one number)
    taxNumber:
      'Mã số thuế từ 10 đến 15 chữ số',
    //   (Tax code must contain 10-15 digits)
    userName:
      'Tên đăng nhập phải có ít nhất 6 kí tự',
    //   (Username must be at least 6 characters)
    userNameFormat:
      'Tên đăng nhập không đúng định dạng. Không được chứa kí tự đặc biệt',
    //   (Invalid username format. Cannot contain special characters)
    totalValue: 'Giá trị phải lớn hơn 0',
    // (Value must be greater than zero)
    mustMatch: 'Mật khẩu không khớp',
    // (Password does not match)
    supplyChainTypesRequired:
      'Bạn cần chọn ít nhất 1 loại tài khoản',
    //   (You need to select at least 1 account type)
    currentDay:
      'Giá trị không được lớn hơn ngày hiện tại',
    //   (Date cannot be greater than current date)
    'Mask error': '',
    codeFormat:
      'Mã không đúng định dạng. Không được chứa kí tự đặc biệt. Bắt đầu và kết thúc bởi chữ hoặc số',
  };
}
