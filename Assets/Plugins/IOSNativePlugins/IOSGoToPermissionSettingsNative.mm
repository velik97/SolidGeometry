extern "C" {
    void OpenSettings () {
        NSURL * url = [NSURL URLWithString: UIApplicationOpenSettingsURLString];
        [[UIApplication sharedApplication] openURL: url];
    }
}