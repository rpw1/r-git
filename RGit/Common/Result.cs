namespace RGit.Common;

public class Result<T, TError>
{
    private T? _value;
    private TError? _error;

    public bool IsSuccess { get; }

    public T Value
    {
        get => IsSuccess ? _value! : throw new InvalidOperationException("Result is not successful.");
        private set => _value = value;
    }

    public TError Error
    {
        get => !IsSuccess ? _error! : throw new InvalidOperationException("Result is successful.");
        private set => _error = value;
    }

    private Result(bool isSuccess, T? value, TError? error) =>
        (IsSuccess, _value, _error) = (isSuccess, value, error);

    public static Result<T, TError> Success(T value) =>
        new(true, value, default);

    public static Result<T, TError> Failure(TError error) =>
        new(false, default, error);
}

public static class ResultExtensions
{
    extension<T1, TError>(Result<T1, TError> result)
    {
        #region Map
        public Result<T2, TError> Map<T2>(Func<T1, T2> map) =>
            result.IsSuccess 
                ? Result<T2, TError>.Success(map(result.Value))
                : Result<T2, TError>.Failure(result.Error);

        public async Task<Result<T2, TError>> MapAsync<T2>(Func<T1, Task<T2>> mapAsync) =>
            result.IsSuccess 
                ? Result<T2, TError>.Success(await mapAsync(result.Value))
                : Result<T2, TError>.Failure(result.Error);
        #endregion

        #region MapError
        public Result<T1, TNewError> MapError<TNewError>(Func<TError, TNewError> map) =>
            result.IsSuccess
                ? Result<T1, TNewError>.Success(result.Value)
                : Result<T1, TNewError>.Failure(map(result.Error));

        public async Task<Result<T1, TNewError>> MapErrorAsync<TNewError>(Func<TError, Task<TNewError>> mapAsync) =>
            result.IsSuccess
                ? Result<T1, TNewError>.Success(result.Value)
                : Result<T1, TNewError>.Failure(await mapAsync(result.Error));
        #endregion

        #region Bind
        public Result<T2, TError> Bind<T2>(Func<T1, Result<T2, TError>> bind) =>
            result.IsSuccess 
                ? bind(result.Value)
                : Result<T2, TError>.Failure(result.Error);

        public async Task<Result<T2, TError>> BindAsync<T2>(Func<T1, Task<Result<T2, TError>>> bindAsync) =>
            result.IsSuccess 
                ? await bindAsync(result.Value)
                : Result<T2, TError>.Failure(result.Error);
        #endregion

        #region Match
        public TResult Match<TResult>(Func<T1, TResult> onSuccess, Func<TError, TResult> onFailure) =>
            result.IsSuccess 
                ? onSuccess(result.Value)
                : onFailure(result.Error);

        public async Task<TResult> MatchAsync<TResult>(Func<T1, Task<TResult>> onSuccessAsync, Func<TError, TResult> onFailure) =>
            result.IsSuccess 
                ? await onSuccessAsync(result.Value)
                : onFailure(result.Error);

        public async Task<TResult> MatchAsync<TResult>(Func<T1, TResult> onSuccess, Func<TError, Task<TResult>> onFailureAsync) =>
            result.IsSuccess 
                ? onSuccess(result.Value)
                : await onFailureAsync(result.Error);

        public async Task<TResult> MatchAsync<TResult>(Func<T1, Task<TResult>> onSuccessAsync, Func<TError, Task<TResult>> onFailureAsync) =>
            result.IsSuccess 
                ? await onSuccessAsync(result.Value)
                : await onFailureAsync(result.Error);
        #endregion
    }

    extension<T1, TError>(Task<Result<T1, TError>> result)
    {
        #region Map
        public async Task<Result<T2, TError>> MapAsync<T2>(Func<T1, T2> map) =>
            (await result).Map(map);

        public async Task<Result<T2, TError>> MapAsync<T2>(Func<T1, Task<T2>> mapAsync) =>
            await (await result).MapAsync(mapAsync);
        #endregion

        #region MapError
        public async Task<Result<T1, TNewError>> MapErrorAsync<TNewError>(Func<TError, TNewError> map) =>
            (await result).MapError(map);

        public async Task<Result<T1, TNewError>> MapErrorAsync<TNewError>(Func<TError, Task<TNewError>> mapAsync) =>
            await (await result).MapErrorAsync(mapAsync);
        #endregion

        #region Bind
        public async Task<Result<T2, TError>> BindAsync<T2>(Func<T1, Result<T2, TError>> bind) =>
            (await result).Bind(bind);

        public async Task<Result<T2, TError>> BindAsync<T2>(Func<T1, Task<Result<T2, TError>>> bindAsync) =>
            await (await result).BindAsync(bindAsync);
        #endregion

        #region Match
        public async Task<TResult> MatchAsync<TResult>(Func<T1, TResult> onSuccess, Func<TError, TResult> onFailure) =>
            (await result).Match(onSuccess, onFailure);

        public async Task<TResult> MatchAsync<TResult>(Func<T1, Task<TResult>> onSuccessAsync, Func<TError, TResult> onFailure) =>
            await (await result).MatchAsync(onSuccessAsync, onFailure);

        public async Task<TResult> MatchAsync<TResult>(Func<T1, TResult> onSuccess, Func<TError, Task<TResult>> onFailureAsync) =>
            await (await result).MatchAsync(onSuccess, onFailureAsync);

        public async Task<TResult> MatchAsync<TResult>(Func<T1, Task<TResult>> onSuccessAsync, Func<TError, Task<TResult>> onFailureAsync) =>
            await (await result).MatchAsync(onSuccessAsync, onFailureAsync);
        #endregion
    }


    

    

    

    

    
}
