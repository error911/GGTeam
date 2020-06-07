# Audio
AudioController

IAudioController, интерфейс предназначенный для общего управлением звуком (вкл\выкл, общая громкость):
IAudioPlayer, интерфейс предназначен для воспроизведения 2д и 3д звуков, и дальнейшего их контроля.
IMusicPlayer, воспроизведение музыки и контроль.

При вызове метода воспроизведения звука или музыки, выдается id, по которому в дальнейшем можно контролировать звук.
Например, выключить его или сменить позицию источника звука, если объект движется.
Есть отдельный метод:
SetAudioListenerToPosition(Vector3 position);
В случае 3d звука и движущегося слушателя необходимо предоставить доступ к контролю его позиции.

IAudioPlayer test = Substitute.For<IAudioPlayer >();
var testClass = new Class(test);

var ac = new AudioController();
    IAudioController iac = ac;
    IAudioPlayer iap = ac;
    IMusicPlayer imp = ac;






















































































    https://habrahabr.ru/post/275017/