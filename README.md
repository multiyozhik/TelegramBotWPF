# TelegramBotWPF

TelegramBotWPF ����������:
- ��� ��������� � �������� ���������, 
- ��� �������� ������� ��������� � ��������� ���� ("MessagesHistory.txt" ����� �������) � ����������� ��� ���������,
- ��� ��������� ������ ��������� ������ (���. ����������� � ��������� ����� ����������),
- ��� ��������� ������� �������� ����� �����.

������ �������� �� ��������� .NET � WPF � �������������� nuget-�������:
- telegram.bot\17.0.0\
- telegram.bot.extensions.polling\1.0.2\

��� �������� �� ������ t.me/print_current_currency_bot.
������ ������, ��������������� https://telegram.me/BotFather, ������ ���� � "Token.txt" � bin-����� �������.

������ API ���������-���� �� ������ https://core.telegram.org/bots/api#available-types.

���� ����� ����� �� ������� ���� �� ������ �� �� �� ������ https://www.cbr-xml-daily.ru/daily_json.js.
�������� � ������� WebClient DownloadString(), �������� json-������, 
������������� �� � Dictionary, �������� ��������� ����� � ������.

������� ���� ���������� ����� ���� � ���������: 
- ��������� (2 �������: "������� ������� ���������" � txt-���� (��������. � ������ � �����.), "������� ���� ������� ���������"), 
- ����� (�� ����� ����������� ���������� ���� �� ������� ��������� ������: ����������, ��������, �����, ����), 
- ����� ����� (�� ����� ����������� ���������� ���� � �������� ������������ ������ - �������� �����, ���.).

� ������� ���� 2 �������:
- ����� - ListBox ������� ���������,
- ������ - TextBox ��� �������� ��������� ���������� ������������ � ������ "���������".

� ������������ MainWindow.cs :
- ���������. ������ ���� TelegamMessageClient ��� �������. � API ����, � 
- ���������. ItemsSource � ��-�� ������. MessageListBox ����. ����.

����� TelegamMessageClient - ��� �������������� � ���������� TelegramBotClient-������� � ����������� ��� ������� API � 
������������� ObservableCollection ������ ��������� � ������ ���� ������. ������ (��� ����. � ������. ����� � �����������).

