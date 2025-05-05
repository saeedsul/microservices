export async function handleRequest<T>({ promise }: { promise: Promise<T>; }): Promise<[T | null, any]> {
    try {
        const data = await promise;
        return [data, null];
    } catch (error) {
        return [null, error];
    }
}
