# Audio
AudioController

IAudioController, ��������� ��������������� ��� ������ ����������� ������ (���\����, ����� ���������):
IAudioPlayer, ��������� ������������ ��� ��������������� 2� � 3� ������, � ����������� �� ��������.
IMusicPlayer, ��������������� ������ � ��������.

��� ������ ������ ��������������� ����� ��� ������, �������� id, �� �������� � ���������� ����� �������������� ����.
��������, ��������� ��� ��� ������� ������� ��������� �����, ���� ������ ��������.
���� ��������� �����:
SetAudioListenerToPosition(Vector3 position);
� ������ 3d ����� � ����������� ��������� ���������� ������������ ������ � �������� ��� �������.

IAudioPlayer test = Substitute.For<IAudioPlayer >();
var testClass = new Class(test);

var ac = new AudioController();
    IAudioController iac = ac;
    IAudioPlayer iap = ac;
    IMusicPlayer imp = ac;






















































































    https://habrahabr.ru/post/275017/