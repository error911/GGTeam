# Audio
AudioController
https://habrahabr.ru/post/275017/

IAudioController, ��������� ��������������� ��� ����� ����������� ������ (���\����, ����� ���������):
IAudioPlayer, ��������� ������������ ��� ��������������� 2� � 3� ������, � ����������� �� ��������.
IMusicPlayer, ��������������� ������ � ��������.

��� ������ ������ ��������������� ����� ��� ������, ����������� �������� �������� ���, �� �������� �� � ���������� ����� �������������� ����.
��������, ��������� ��� ��� ������� ������� ��������� �����, ���� ������ ��������.
��������� ������� �����:
SetAudioListenerToPosition(Vector3 position);
� ������ 3d ����� � ����������� ��������� ���������� ������������ ������ � �������� ��� �������.


IAudioPlayer test = Substitute.For<IAudioPlayer >();
var testClass = new Class(test);

var ac = new AudioController();
    IAudioController iac = ac;
    IAudioPlayer iap = ac;
    IMusicPlayer imp = ac;