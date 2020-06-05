# Audio
AudioController
https://habrahabr.ru/post/275017/

IAudioController, интерфейс предназначенный для общим управлением звуком (вкл\выкл, общая громкость):
IAudioPlayer, интерфейс предназначен для воспроизведения 2д и 3д звуков, и дальнейшего их контроля.
IMusicPlayer, воспроизведение музыки и контроль.

При вызове метода воспроизведения звука или музыки, потребителю выдается числовой код, по которому он в дальнейшем может контролировать звук.
Например, выключить его или сменить позицию источника звука, если объект движется.
Отдельным методом стоит:
SetAudioListenerToPosition(Vector3 position);
В случае 3d звука и движущегося слушателя необходимо предоставить доступ к контролю его позиции.


IAudioPlayer test = Substitute.For<IAudioPlayer >();
var testClass = new Class(test);

var ac = new AudioController();
    IAudioController iac = ac;
    IAudioPlayer iap = ac;
    IMusicPlayer imp = ac;