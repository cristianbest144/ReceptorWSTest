using DataAccessLayer.Repository;

namespace BusinessLogicLayer.Services
{
    public class SettingStorageServices
    {
        public readonly SettingStorageRepository _settingStorageRepository;
        public SettingStorageServices() {
            _settingStorageRepository =  new SettingStorageRepository();
        }

        public string GetKey(string key) { return null; }
    }
}
